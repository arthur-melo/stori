using Microsoft.EntityFrameworkCore;
using Server.API.Models.Context;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class TokenRepository(StoriContext context) : ITokenRepository
{
  private readonly StoriContext _context = context;

  public async Task<Token?> SaveRefreshTokenAsync(Token token)
  {
    var tokenAlreadyExists = await _context
      .Tokens.AsNoTracking()
      .FirstOrDefaultAsync(t => t.Id == token.Id);

    if (tokenAlreadyExists is not null)
    {
      return null;
    }

    var result = await _context.Tokens.AddAsync(token);
    await _context.SaveChangesAsync();

    return result.Entity;
  }

  public async Task<Token?> GetRefreshTokenAsync(string refreshToken)
  {
    var token = await _context
      .Tokens.AsNoTracking()
      .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

    if (token is null)
    {
      return null;
    }

    return token;
  }

  public async Task<Token?> GetRefreshTokenByIdAsync(int userId)
  {
    var token = await _context.Tokens.AsNoTracking().FirstOrDefaultAsync(t => t.Id == userId);

    if (token is null)
    {
      return null;
    }

    return token;
  }

  public async Task RevokeRefreshTokenAsync(Token token)
  {
    var contextToken = await _context
      .Tokens.AsNoTracking()
      .FirstOrDefaultAsync(t => t.Id == token.Id);

    if (contextToken is null)
    {
      return;
    }

    _context.Tokens.Remove(contextToken);

    await _context.SaveChangesAsync();
  }
}
