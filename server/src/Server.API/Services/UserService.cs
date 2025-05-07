using Microsoft.Extensions.Options;
using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Options;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class UserService(
  IUserRepository userRepository,
  IImageService imageService,
  IEncryptionService encryptionService,
  IOptions<FileUploadOptions> fileUploadOptions
) : IUserService
{
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IImageService _imageService = imageService;
  private readonly IEncryptionService _encryptionService = encryptionService;
  private readonly FileUploadOptions _fileUploadOptions = fileUploadOptions.Value;

  public async Task<Envelope<UserAuthorizedResponse>> GetUserByIdAsync(int id)
  {
    var userResponse = await _userRepository.GetUserResponseByIdAsync(id);

    if (userResponse is null)
    {
      throw new NotFoundException("Invalid authed user.");
    }

    return new Envelope<UserAuthorizedResponse>([userResponse]);
  }

  public async Task<Envelope<UserUnauthorizedResponse>> GetUserByUsernameAsync(string username)
  {
    var userResponse = await _userRepository.GetUserByUsernameAsync(username);

    if (userResponse is null)
    {
      throw new NotFoundException("User not found.");
    }

    return new Envelope<UserUnauthorizedResponse>([userResponse]);
  }

  public async Task<Envelope<UserAuthorizedResponse>> PatchUserAsync(
    int id,
    string username,
    string? newEmail,
    string? newPassword,
    string? newUsername,
    string? newName
  )
  {
    var user = await _userRepository.GetUserByIdAsync(id);

    if (user is null)
    {
      throw new NotFoundException("Invalid user.");
    }

    if (user.Username != username)
    {
      throw new ValidationException("The given username does not match the current user.");
    }

    if (!string.IsNullOrEmpty(newUsername))
    {
      var isValidUsername = await _userRepository.IsUsernameInUseAsync(newUsername);

      if (isValidUsername)
      {
        throw new ValidationException($"The username: \"{newUsername}\" is already in use.");
      }
    }

    if (!string.IsNullOrEmpty(newEmail))
    {
      var isValidEmail = await _userRepository.IsEmailInUseAsync(newEmail);

      if (isValidEmail)
      {
        throw new ValidationException($"The email: \"{newEmail}\" is already in use.");
      }
    }

    string? hashedNewPassword = null;
    if (!string.IsNullOrEmpty(newPassword))
    {
      hashedNewPassword = _encryptionService.HashPassword(newPassword);
    }

    var userResponse = await _userRepository.PatchUserAsync(
      id,
      newEmail,
      hashedNewPassword,
      newUsername,
      newName,
      null
    );

    if (userResponse is null)
    {
      throw new Exception("Error patching database user");
    }

    return userResponse;
  }

  public async Task<string> PostUserPhotoAsync(int id, string username, IFormFile profileImg)
  {
    var user = await _userRepository.GetUserByIdAsync(id);

    if (user is null)
    {
      throw new NotFoundException("Invalid user.");
    }

    if (user.Username != username)
    {
      throw new ValidationException("The given username does not match the current user.");
    }

    // Remove previous image, if it exists.
    if (user.ProfileImg is not null)
    {
      if (_imageService.ValidateDirectory(_fileUploadOptions.Path))
      {
        _imageService.DeleteImage(user.ProfileImg);
      }
    }

    // Save new image to static files dir
    var fileName = await _imageService.ProcessImageAsync(profileImg);

    // Assigns the profile picture to the user
    await _userRepository.PatchUserAsync(id, null, null, null, null, fileName);

    return fileName;
  }

  public async Task RemoveUserPhotoAsync(int id, string username)
  {
    // Verifies that the given username exists, and it belongs to the right id.
    var user = await _userRepository.GetUserByIdAsync(id);

    if (user is null)
    {
      throw new ValidationException("Invalid current user.");
    }

    var requestedUser = await _userRepository.GetUserByUsernameAsync(username);

    if (requestedUser is null)
    {
      throw new NotFoundException("Invalid user.");
    }

    if (user.Username != requestedUser.username)
    {
      throw new ValidationException("The given username does not match the current user.");
    }

    await _userRepository.RemoveUserPhotoAsync(id);

    if (_imageService.ValidateDirectory(_fileUploadOptions.Path))
    {
      _imageService.DeleteImage(user.ProfileImg!);
    }
  }
}
