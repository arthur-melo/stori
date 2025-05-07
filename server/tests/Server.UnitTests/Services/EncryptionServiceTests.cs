using FluentAssertions;
using Server.API.Services;

namespace Server.UnitTests.Services;

using BCrypt.Net;

public class EncryptionServiceTests
{
  [Fact]
  public void VerifyPassword_ValidParameters_ReturnsTrue()
  {
    // Arrange
    var password = "password";
    var hashedPassword = BCrypt.HashPassword(password);

    var encryptionService = new EncryptionService();

    // Act
    var response = encryptionService.VerifyPassword(password, hashedPassword);

    // Assert
    response.Should().BeTrue();
  }

  [Fact]
  public void VerifyPassword_InvalidParameters_ReturnsFalse()
  {
    // Arrange
    var password = "password";
    var hashedPassword = BCrypt.HashPassword(password);

    var encryptionService = new EncryptionService();

    // Act
    var response = encryptionService.VerifyPassword(password, hashedPassword + "invalid");

    // Assert
    response.Should().BeFalse();
  }

  [Fact]
  public void HashPassword_ValidParameters_ReturnsHashedResponse()
  {
    // Arrange
    var password = "password";

    var encryptionService = new EncryptionService();

    // Act
    var response = encryptionService.HashPassword(password);

    var isValid = BCrypt.Verify(password, response);

    // Assert
    response.Should().NotBeNull();
    isValid.Should().BeTrue();
  }
}
