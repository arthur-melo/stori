using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.API.Models;
using Server.API.Options;

namespace Server.API.Extensions;

public static class IdentityServiceExtensions
{
  public static void AddIdentityServices(this IServiceCollection services, IConfiguration config)
  {
    var _jwtOptions = config.GetSection(nameof(JWTOptions)).Get<JWTOptions>();

    if (_jwtOptions is null)
    {
      throw new Exception("No JWTOptions appsettings defined.");
    }

    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));

    var signingCredentials = new SigningCredentials(
      securityKey,
      SecurityAlgorithms.HmacSha256Signature
    );

    var signingConfiguration = new SigningConfiguration(securityKey, signingCredentials);

    services.AddSingleton(signingConfiguration);

    services
      .AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(jwtBearerOptions =>
      {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = _jwtOptions.Issuer,
          ValidAudience = _jwtOptions.Audience,
          IssuerSigningKey = signingConfiguration.securityKey,
          ClockSkew = TimeSpan.Zero,
        };
      });
  }
}
