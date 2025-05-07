namespace Server.API.Options;

public sealed class JWTOptions
{
  public required string Secret { get; set; } = string.Empty;
  public required string Issuer { get; set; } = string.Empty;
  public required string Audience { get; set; } = string.Empty;
  public required int AccessTokenExpiration { get; set; }
  public required int RefreshTokenExpiration { get; set; }
}
