using Server.API.Models;
using Server.API.Models.Dtos.Responses;

namespace Server.API.Services.Interfaces;

public interface IUserService
{
  public Task<Envelope<UserAuthorizedResponse>> GetUserByIdAsync(int id);
  public Task<Envelope<UserUnauthorizedResponse>> GetUserByUsernameAsync(string username);

  public Task<Envelope<UserAuthorizedResponse>> PatchUserAsync(
    int id,
    string username,
    string? newEmail,
    string? newPassword,
    string? newUsername,
    string? newName
  );

  public Task<string> PostUserPhotoAsync(int id, string username, IFormFile profileImg);

  public Task RemoveUserPhotoAsync(int id, string username);
}
