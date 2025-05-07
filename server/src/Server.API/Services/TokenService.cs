using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

using System.Security.Claims;
using Server.API.Exceptions;
using Server.API.Options;
using Server.API.Repositories.Interfaces;

public class TokenService(
  IOptions<JWTOptions> jwtOptions,
  SigningConfiguration signingConfiguration,
  ITokenRepository tokenRepository,
  IEncryptionService encryptionService,
  IDateTimeService dateTimeService
) : ITokenService
{
  private readonly JWTOptions _jwtOptions = jwtOptions.Value;
  private readonly SigningConfiguration _signingConfiguration = signingConfiguration;
  private readonly ITokenRepository _tokenRepository = tokenRepository;
  private readonly IEncryptionService _encryptionService = encryptionService;
  private readonly IDateTimeService _dateTimeService = dateTimeService;

  public async Task<(AccessToken, RefreshToken)> CreateTokensAsync(User user)
  {
    var refreshToken = CreateRefreshToken();
    var accessToken = CreateAccessToken(user);

    var newToken = new Token
    {
      Id = user.Id,
      RefreshToken = refreshToken.token,
      Expiration = new DateTime(refreshToken.expiration),
    };

    var tokenEntity = await _tokenRepository.SaveRefreshTokenAsync(newToken);

    // Token already exists on the DB, remove it and try again
    if (tokenEntity is null)
    {
      var token = await _tokenRepository.GetRefreshTokenByIdAsync(user.Id);

      if (token is null)
      {
        throw new Exception("Error retrieving saved refresh token from the database.");
      }

      await _tokenRepository.RevokeRefreshTokenAsync(token);

      var newTokenEntity = await _tokenRepository.SaveRefreshTokenAsync(newToken);

      if (newTokenEntity is null)
      {
        throw new Exception("Error saving refresh token to database.");
      }
    }

    return (accessToken, refreshToken);
  }

  public AccessToken CreateAccessToken(User user)
  {
    var accessTokenExpiration = _dateTimeService
      .Now()
      .ToUniversalTime()
      .AddMinutes(_jwtOptions.AccessTokenExpiration);

    var securityToken = new JwtSecurityToken(
      issuer: _jwtOptions.Issuer,
      audience: _jwtOptions.Audience,
      expires: accessTokenExpiration,
      claims: GetClaims(user),
      notBefore: _dateTimeService.Now().ToUniversalTime(),
      signingCredentials: _signingConfiguration.signingCredentials
    );

    var handler = new JwtSecurityTokenHandler();
    var accessToken = handler.WriteToken(securityToken);

    return new AccessToken(accessToken, accessTokenExpiration.Ticks);
  }

  private RefreshToken CreateRefreshToken()
  {
    var refreshToken = new RefreshToken(
      token: _encryptionService.HashPassword(Guid.NewGuid().ToString()),
      expiration: _dateTimeService
        .Now()
        .ToUniversalTime()
        .AddMinutes(_jwtOptions.RefreshTokenExpiration)
        .Ticks
    );

    return refreshToken;
  }

  public async Task<Token> GetRefreshTokenAsync(string refreshToken)
  {
    var token = await _tokenRepository.GetRefreshTokenAsync(refreshToken);

    if (token is null)
    {
      throw new NotFoundException("Token not found on the database.");
    }

    return token;
  }

  public async Task RevokeRefreshTokenAsync(string refreshToken)
  {
    var token = await _tokenRepository.GetRefreshTokenAsync(refreshToken);

    if (token is null)
    {
      throw new NotFoundException("Token not found on the database");
    }

    await _tokenRepository.RevokeRefreshTokenAsync(token);
  }

  private IEnumerable<Claim> GetClaims(User user)
  {
    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    };

    return claims;
  }
}
