using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Repositories.Interfaces;

public interface IUserRepository
{
  public Task<User> SaveUserAsync(User user);

  public Task<bool> IsEmailInUseAsync(string email);

  public Task<bool> IsUsernameInUseAsync(string username);

  public Task<bool> IsUserInDatabaseAsync(int userId);

  public Task<User?> GetUserByIdAsync(int id);

  public Task<UserAuthorizedResponse?> GetUserResponseByIdAsync(int id);

  public Task<User?> GetUserByEmailAsync(string email);

  public Task<UserUnauthorizedResponse?> GetUserByUsernameAsync(string username);

  public Task<Envelope<UserAuthorizedResponse>?> PatchUserAsync(
    int Id,
    string? Email,
    string? Password,
    string? Username,
    string? Name,
    string? ProfileImg
  );
  public Task<User?> RemoveUserPhotoAsync(int userId);
}
