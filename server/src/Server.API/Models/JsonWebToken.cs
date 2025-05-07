using Microsoft.IdentityModel.Tokens;

namespace Server.API.Models;

public record AccessToken(string token, long expiration);

public record RefreshToken(string token, long expiration);

public record SigningConfiguration(
  SymmetricSecurityKey securityKey,
  SigningCredentials signingCredentials
);
