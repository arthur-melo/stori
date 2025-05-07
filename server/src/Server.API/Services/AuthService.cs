using Server.API.Exceptions;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

using Server.API.Models.Dtos.Responses;

public class AuthService(
  IUserRepository userRepository,
  ITokenService tokenService,
  IEncryptionService encryptionService,
  IDateTimeService dateTimeService
) : IAuthService
{
  private readonly IUserRepository _userRepository = userRepository;
  private readonly ITokenService _tokenService = tokenService;
  private readonly IEncryptionService _encryptionService = encryptionService;
  private readonly IDateTimeService _dateTimeService = dateTimeService;

  public async Task<TokenResponse> SigninAsync(string email, string password)
  {
    var user = await _userRepository.GetUserByEmailAsync(email);

    if (user is null)
    {
      throw new NotFoundException("No user found with the given email.");
    }

    var isValidCredentials = _encryptionService.VerifyPassword(password, user.Password);

    if (!isValidCredentials)
    {
      throw new ValidationException("Invalid password.");
    }

    var (accessToken, refreshToken) = await _tokenService.CreateTokensAsync(user);

    var response = new TokenResponse(accessToken, refreshToken);

    return response;
  }

  public async Task<string> SignupAsync(string username, string name, string email, string password)
  {
    if (await _userRepository.IsEmailInUseAsync(email))
    {
      throw new ValidationException($"The email: \"{email}\" is already in use.");
    }

    if (await _userRepository.IsUsernameInUseAsync(username))
    {
      throw new ValidationException($"The username: \"{username}\" is already in use.");
    }

    var hashedPassword = _encryptionService.HashPassword(password);

    var newUser = new User
    {
      Username = username,
      Name = name,
      Email = email,
      Password = hashedPassword,
      CreatedAt = _dateTimeService.Now(),
    };

    var savedUser = await _userRepository.SaveUserAsync(newUser);

    return savedUser.Username;
  }

  public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
  {
    var token = await _tokenService.GetRefreshTokenAsync(refreshToken);

    var isExpired = _dateTimeService.Now().ToUniversalTime().Ticks > token.Expiration.Ticks;

    if (isExpired)
    {
      throw new ValidationException("The given token is expired, please log in again.");
    }

    var user = await _userRepository.GetUserByIdAsync(token.Id);

    if (user is null)
    {
      throw new ValidationException("Invalid user.");
    }

    var (newAccessToken, newRefreshToken) = await _tokenService.CreateTokensAsync(user);

    var response = new TokenResponse(newAccessToken, newRefreshToken);

    return response;
  }

  public async Task RevokeRefreshTokenAsync(string refreshToken)
  {
    await _tokenService.RevokeRefreshTokenAsync(refreshToken);
  }
}
