using FluentAssertions;
using Moq;
using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services;
using Server.API.Services.Interfaces;

namespace Server.UnitTests.Services;

public class AuthServicesTests
{
  [Fact]
  public async Task SigninAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var email = "user@example.com";
    var password = "password";
    var mockUserRepository = new Mock<IUserRepository>();
    var mockTokenService = new Mock<ITokenService>();
    var mockEncryptionService = new Mock<IEncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByEmailAsync(It.IsAny<string>()).Result)
      .Returns(
        new User
        {
          Id = 1,
          Email = email,
          Password = password,
        }
      );

    mockEncryptionService
      .Setup(es => es.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
      .Returns(true);

    mockTokenService
      .Setup(ts => ts.CreateTokensAsync(It.IsAny<User>()).Result)
      .Returns((new AccessToken("", 0), new RefreshToken("", 0)));

    var authService = new AuthService(
      mockUserRepository.Object,
      mockTokenService.Object,
      mockEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var (accessToken, refreshToken) = await authService.SigninAsync(email, password);

    // Assert
    accessToken.Should().NotBeNull();
    refreshToken.Should().NotBeNull();
  }

  [Fact]
  public async Task SigninAsync_InvalidEmail_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubTokenService = new Mock<ITokenService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByEmailAsync(It.IsAny<string>()).Result);

    var authService = new AuthService(
      mockUserRepository.Object,
      stubTokenService.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await authService.SigninAsync("", "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task SigninAsync_InvalidPassword_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubTokenService = new Mock<ITokenService>();
    var mockEncryptionService = new Mock<IEncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByEmailAsync(It.IsAny<string>()).Result)
      .Returns(
        new User
        {
          Id = 1,
          Email = "",
          Password = "",
        }
      );

    mockEncryptionService
      .Setup(es => es.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
      .Returns(false);

    var authService = new AuthService(
      mockUserRepository.Object,
      stubTokenService.Object,
      mockEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await authService.SigninAsync("", "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task SignupAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var username = "username";
    var mockUserRepository = new Mock<IUserRepository>();
    var stubTokenService = new Mock<ITokenService>();
    var mockEncryptionService = new Mock<IEncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsEmailInUseAsync(It.IsAny<string>()).Result).Returns(false);
    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(false);

    mockUserRepository
      .Setup(ur => ur.SaveUserAsync(It.IsAny<User>()).Result)
      .Returns(new User() { Username = username });

    mockEncryptionService.Setup(es => es.HashPassword(It.IsAny<string>())).Returns("");

    var authService = new AuthService(
      mockUserRepository.Object,
      stubTokenService.Object,
      mockEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var newUser = await authService.SignupAsync("", "", "", "");

    // Assert
    newUser.Should().Be(username);
    mockUserRepository.Verify(ur => ur.SaveUserAsync(It.IsAny<User>()), Times.Once());
  }

  [Fact]
  public async Task SignupAsync_EmailInUse_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubTokenService = new Mock<ITokenService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsEmailInUseAsync(It.IsAny<string>()).Result).Returns(true);

    var authService = new AuthService(
      mockUserRepository.Object,
      stubTokenService.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await authService.SignupAsync("", "", "", "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task SignupAsync_UsernameInUse_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubTokenService = new Mock<ITokenService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsEmailInUseAsync(It.IsAny<string>()).Result).Returns(false);
    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    var authService = new AuthService(
      mockUserRepository.Object,
      stubTokenService.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await authService.SignupAsync("", "", "", "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RefreshTokenAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var mockTokenService = new Mock<ITokenService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result).Returns(new User());

    mockTokenService
      .Setup(ts => ts.GetRefreshTokenAsync(It.IsAny<string>()).Result)
      .Returns(new Token() { RefreshToken = "", Expiration = DateTime.UnixEpoch.AddSeconds(1) });

    mockTokenService
      .Setup(ts => ts.CreateAccessToken(It.IsAny<User>()))
      .Returns(new AccessToken("", 0));

    mockDateTimeService.Setup(dts => dts.Now()).Returns(DateTime.UnixEpoch);

    var authService = new AuthService(
      mockUserRepository.Object,
      mockTokenService.Object,
      stubEncryptionService.Object,
      mockDateTimeService.Object
    );

    // Act
    var response = await authService.RefreshTokenAsync("");

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task RefreshTokenAsync_ExpiredToken_ThrowsError()
  {
    // Arrange
    var stubUserRepository = new Mock<IUserRepository>();
    var mockTokenService = new Mock<ITokenService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockTokenService
      .Setup(ts => ts.GetRefreshTokenAsync(It.IsAny<string>()).Result)
      .Returns(new Token() { RefreshToken = "", Expiration = DateTime.UnixEpoch });

    mockDateTimeService.Setup(dts => dts.Now()).Returns(DateTime.UnixEpoch.AddSeconds(1));

    var authService = new AuthService(
      stubUserRepository.Object,
      mockTokenService.Object,
      stubEncryptionService.Object,
      mockDateTimeService.Object
    );

    // Act
    var act = async () => await authService.RefreshTokenAsync("");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RefreshTokenAsync_InvalidEmail_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var mockTokenService = new Mock<ITokenService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByEmailAsync(It.IsAny<string>()).Result);

    mockTokenService
      .Setup(ts => ts.GetRefreshTokenAsync(It.IsAny<string>()).Result)
      .Returns(new Token() { RefreshToken = "", Expiration = DateTime.UnixEpoch.AddSeconds(1) });

    mockDateTimeService.Setup(dts => dts.Now()).Returns(DateTime.UnixEpoch);

    var authService = new AuthService(
      mockUserRepository.Object,
      mockTokenService.Object,
      stubEncryptionService.Object,
      mockDateTimeService.Object
    );

    // Act
    var act = async () => await authService.RefreshTokenAsync("");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RevokeRefreshTokenAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var stubUserRepository = new Mock<IUserRepository>();
    var stubTokenService = new Mock<ITokenService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    var authService = new AuthService(
      stubUserRepository.Object,
      stubTokenService.Object,
      stubEncryptionService.Object,
      stubDateTimeService.Object
    );

    // Act
    await authService.RevokeRefreshTokenAsync("");

    // Assert
    stubTokenService.Verify(ts => ts.RevokeRefreshTokenAsync(It.IsAny<string>()), Times.Once());
  }
}
