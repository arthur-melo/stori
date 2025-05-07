using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.API.Options;
using Server.API.Repositories.Interfaces;
using Server.API.Services;
using Server.API.Services.Interfaces;

namespace Server.UnitTests.Services;

public class TokenServiceTests
{
  private IOptions<JWTOptions> GenerateOptions(
    int accessTokenExpiration = 1,
    int refreshTokenExpiration = 1,
    string audience = "audience",
    string issuer = "issuer"
  )
  {
    return Options.Create(
      new JWTOptions()
      {
        Secret = "test-security-key-test-security-key",
        AccessTokenExpiration = accessTokenExpiration,
        Audience = audience,
        Issuer = issuer,
        RefreshTokenExpiration = refreshTokenExpiration,
      }
    );
  }

  private SigningConfiguration GenerateSigningConfiguration(string secret)
  {
    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

    var signingCredentials = new SigningCredentials(
      securityKey,
      SecurityAlgorithms.HmacSha256Signature
    );

    var signingConfiguration = new SigningConfiguration(securityKey, signingCredentials);

    return signingConfiguration;
  }

  [Fact]
  public async Task CreateTokensAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var userId = 1;
    var accessTokenExpiration = 1;
    var refreshTokenExpiration = 1;
    var time = DateTime.UnixEpoch;
    var audience = "audience";
    var issuer = "issuer";
    var mockIOptions = GenerateOptions(
      accessTokenExpiration,
      refreshTokenExpiration,
      audience,
      issuer
    );
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository
      .Setup(tr => tr.SaveRefreshTokenAsync(It.IsAny<Token>()).Result)
      .Returns(new Token() { RefreshToken = "" });

    mockDateTimeService.Setup(dts => dts.Now()).Returns(time);

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      mockDateTimeService.Object
    );

    // Act
    var (accessToken, refreshToken) = await tokenService.CreateTokensAsync(
      new User() { Id = userId }
    );

    var handler = new JwtSecurityTokenHandler();
    var jwtSecurityToken = handler.ReadJwtToken(accessToken.token);

    // Assert
    jwtSecurityToken.Audiences.First(a => a.Equals(audience)).Should().NotBeNull();
    jwtSecurityToken.Issuer.Equals(issuer).Should().BeTrue();
    jwtSecurityToken.Claims.First(c => c.Type == "sub").Value.Should().Be(userId.ToString());
    jwtSecurityToken
      .Claims.First(c => c.Type == "nbf")
      .Value.Should()
      .Be(time == DateTime.UnixEpoch ? "0" : time.Ticks.ToString());

    accessToken.Should().NotBeNull();
    accessToken.expiration.Should().Be(time.AddMinutes(accessTokenExpiration).Ticks);

    refreshToken.Should().NotBeNull();
    refreshToken.expiration.Should().Be(time.AddMinutes(refreshTokenExpiration).Ticks);
  }

  [Fact]
  public async Task CreateTokensAsync_TokenExistsOnDatabase_ReturnsValidResponse()
  {
    // Arrange
    var mockIOptions = GenerateOptions();
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository
      .SetupSequence(tr => tr.SaveRefreshTokenAsync(It.IsAny<Token>()).Result)
      .Returns(() => null)
      .Returns(new Token());

    mockTokenRepository
      .Setup(tr => tr.GetRefreshTokenByIdAsync(It.IsAny<int>()).Result)
      .Returns(new Token());
    mockTokenRepository.Setup(tr => tr.RevokeRefreshTokenAsync(It.IsAny<Token>()));

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var (accessToken, refreshToken) = await tokenService.CreateTokensAsync(new User());

    // Assert
    accessToken.Should().NotBeNull();
    refreshToken.Should().NotBeNull();
    mockTokenRepository.Verify(tr => tr.SaveRefreshTokenAsync(It.IsAny<Token>()), Times.Exactly(2));
  }

  [Fact]
  public async Task CreateTokensAsync_TokenExistsErrorRetrieving_ThrowsError()
  {
    // Arrange
    var mockIOptions = GenerateOptions();
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository
      .Setup(tr => tr.SaveRefreshTokenAsync(It.IsAny<Token>()).Result)
      .Returns(() => null);

    mockTokenRepository.Setup(tr => tr.GetRefreshTokenByIdAsync(It.IsAny<int>()).Result);

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await tokenService.CreateTokensAsync(new User());

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task CreateTokensAsync_ErrorSavingTokenOnDatabase_ThrowsError()
  {
    // Arrange
    var mockIOptions = GenerateOptions();
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository
      .SetupSequence(tr => tr.SaveRefreshTokenAsync(It.IsAny<Token>()).Result)
      .Returns(() => null)
      .Returns(() => null);

    mockTokenRepository
      .Setup(tr => tr.GetRefreshTokenByIdAsync(It.IsAny<int>()).Result)
      .Returns(new Token());

    mockTokenRepository.Setup(tr => tr.RevokeRefreshTokenAsync(It.IsAny<Token>()));

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await tokenService.CreateTokensAsync(new User());

    // Assert
    await act.Should().ThrowAsync<Exception>();
    mockTokenRepository.Verify(tr => tr.RevokeRefreshTokenAsync(It.IsAny<Token>()), Times.Once());
  }

  [Fact]
  public void CreateAccessToken_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var accessTokenExpiration = 1;
    var time = DateTime.UnixEpoch;
    var mockIOptions = GenerateOptions(accessTokenExpiration);
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var stubTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockDateTimeService.Setup(dts => dts.Now()).Returns(time);

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      stubTokenRepository.Object,
      stubEncryptionService.Object,
      mockDateTimeService.Object
    );

    // Act
    var accessToken = tokenService.CreateAccessToken(new User());

    // Assert
    accessToken.Should().NotBeNull();
    accessToken.expiration.Should().Be(time.AddMinutes(accessTokenExpiration).Ticks);
  }

  [Fact]
  public async Task GetRefreshTokenAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockIOptions = GenerateOptions();
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository
      .Setup(tr => tr.GetRefreshTokenAsync(It.IsAny<string>()).Result)
      .Returns(new Token() { RefreshToken = "" });

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var refreshToken = await tokenService.GetRefreshTokenAsync("");

    // Assert
    refreshToken.Should().NotBeNull();
  }

  [Fact]
  public async Task GetRefreshTokenAsync_InvalidRefreshToken_ThrowsError()
  {
    // Arrange
    var mockIOptions = GenerateOptions();
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository.Setup(tr => tr.GetRefreshTokenAsync(It.IsAny<string>()).Result);

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await tokenService.GetRefreshTokenAsync("");

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task RevokeRefreshTokenAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockIOptions = GenerateOptions();
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository
      .Setup(tr => tr.GetRefreshTokenAsync(It.IsAny<string>()).Result)
      .Returns(new Token() { RefreshToken = "" });

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    await tokenService.RevokeRefreshTokenAsync("");

    // Assert
    mockTokenRepository.Verify(tr => tr.GetRefreshTokenAsync(It.IsAny<string>()), Times.Once());
  }

  [Fact]
  public async Task RevokeRefreshTokenAsync_InvalidRefreshToken_ThrowsError()
  {
    // Arrange
    var mockIOptions = GenerateOptions();
    var mockSigningConfiguration = GenerateSigningConfiguration(mockIOptions.Value.Secret);
    var mockTokenRepository = new Mock<ITokenRepository>();
    var stubEncryptionService = new Mock<EncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockTokenRepository.Setup(tr => tr.GetRefreshTokenAsync(It.IsAny<string>()).Result);

    var tokenService = new TokenService(
      mockIOptions,
      mockSigningConfiguration,
      mockTokenRepository.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await tokenService.RevokeRefreshTokenAsync("");

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }
}
