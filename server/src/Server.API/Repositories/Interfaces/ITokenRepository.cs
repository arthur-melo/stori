using Server.API.Models.Entities;

namespace Server.API.Repositories.Interfaces;

public interface ITokenRepository
{
  public Task<Token?> SaveRefreshTokenAsync(Token token);
  public Task<Token?> GetRefreshTokenAsync(string refreshToken);
  public Task<Token?> GetRefreshTokenByIdAsync(int userId);
  public Task RevokeRefreshTokenAsync(Token token);
}
