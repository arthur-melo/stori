using Server.API.Models;
using Server.API.Models.Entities;

namespace Server.API.Services.Interfaces;

public interface ITokenService
{
  public Task<(AccessToken, RefreshToken)> CreateTokensAsync(User user);
  public AccessToken CreateAccessToken(User user);
  public Task<Token> GetRefreshTokenAsync(string refreshToken);
  public Task RevokeRefreshTokenAsync(string refreshToken);
}
