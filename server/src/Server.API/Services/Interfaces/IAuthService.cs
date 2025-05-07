using Server.API.Models.Dtos.Responses;

namespace Server.API.Services.Interfaces;

public interface IAuthService
{
  public Task<TokenResponse> SigninAsync(string email, string password);
  public Task<string> SignupAsync(string username, string name, string email, string password);
  public Task<TokenResponse> RefreshTokenAsync(string refreshToken);
  public Task RevokeRefreshTokenAsync(string refreshToken);
}
