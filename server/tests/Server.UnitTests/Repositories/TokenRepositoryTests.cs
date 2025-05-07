using FluentAssertions;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class TokenRepositoryTests : BaseTests
{
  [Fact]
  public async Task SaveRefreshTokenAsync_SaveToken_ReturnsValidResponse()
  {
    // Arrange
    var id = 1;

    var user = new User()
    {
      Id = id,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var tokenRepository = new TokenRepository(_context);

    var token = new Token()
    {
      Id = id,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    // Act
    var result = await tokenRepository.SaveRefreshTokenAsync(token);

    // Assert
    result.Should().NotBeNull();
    _context.Tokens.FirstOrDefault(t => t.Id == token.Id).Should().NotBe(null);
    _context.Tokens.Should().HaveCount(1);
  }

  [Fact]
  public async Task SaveRefreshTokenAsync_SaveDuplicatedToken_ReturnsNullResponse()
  {
    // Arrange
    var id = 1;

    var user = new User()
    {
      Id = id,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var token = new Token()
    {
      Id = id,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    _context.Tokens.Add(token);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var tokenRepository = new TokenRepository(_context);

    var newToken = new Token()
    {
      Id = id,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    // Act
    var result = await tokenRepository.SaveRefreshTokenAsync(newToken);

    // Assert
    result.Should().BeNull();
    _context.Tokens.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetTokenAsync_GetValidToken_ReturnsValidResponse()
  {
    // Arrange
    var id = 1;

    var token = new Token()
    {
      Id = id,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    _context.Tokens.Add(token);

    var user = new User()
    {
      Id = id,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    user.Token = token;
    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var tokenRepository = new TokenRepository(_context);

    // Act
    var result = await tokenRepository.GetRefreshTokenAsync("");

    // Assert
    result.Should().NotBeNull();
  }

  [Fact]
  public async Task GetTokenAsync_GetInvalidToken_ReturnsNullResponse()
  {
    // Arrange
    var id = 1;

    var user = new User()
    {
      Id = id,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var token = new Token()
    {
      Id = id,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    _context.Tokens.Add(token);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var tokenRepository = new TokenRepository(_context);

    var newToken = new Token()
    {
      Id = id,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    // Act
    var result = await tokenRepository.GetRefreshTokenAsync("invalid");

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task GetTokenByIdAsync_GetValidToken_ReturnsValidResponse()
  {
    // Arrange
    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var token = new Token()
    {
      Id = 1,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    _context.Tokens.Add(token);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var tokenRepository = new TokenRepository(_context);

    var tokenDb = _context.Tokens.First();

    // Act
    var result = await tokenRepository.GetRefreshTokenByIdAsync(tokenDb.Id);

    // Assert
    result.Should().NotBeNull();
  }

  [Fact]
  public async Task GetTokenByIdAsync_GetInvalidToken_ReturnsNullResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var tokenRepository = new TokenRepository(_context);

    // Act
    var result = await tokenRepository.GetRefreshTokenByIdAsync(1);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task RevokeRefreshTokenAsync_RemoveToken_ReturnsNoResponse()
  {
    // Arrange
    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var token = new Token()
    {
      Id = 1,
      Expiration = DateTime.UtcNow,
      RefreshToken = "",
    };

    _context.Tokens.Add(token);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var tokenRepository = new TokenRepository(_context);

    // Act
    _context.ChangeTracker.Clear();
    await tokenRepository.RevokeRefreshTokenAsync(token);

    // Assert
    _context.Tokens.Should().HaveCount(0);
  }
}
