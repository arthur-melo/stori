using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class SigninRequestValidatorTests
{
  [Fact]
  public void SigninRequestValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new SigninRequest("example@email.com", "password");
    var validator = new SigninRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("invalid-email.com")]
  [InlineData("invalid")]
  [InlineData("@invalid.com")]
  public void SigninRequestValidator_InvalidEmailValues_ReturnsError(string? email)
  {
    // Arrange
    var model = new SigninRequest(email, "password");
    var validator = new SigninRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.email).Only();
    ;
  }

  [Fact]
  public void SigninRequestValidator_InvalidEmailLengthValues_ReturnsError()
  {
    // Arrange
    // Total: 257 characters for email, limit is 256.
    var longEmail = string.Concat(new string('a', 247), "@email.com");
    var model = new SigninRequest(longEmail, "password");
    var validator = new SigninRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.email).Only();
    ;
  }

  [Theory]
  [InlineData(null)]
  [InlineData("    ")]
  [InlineData("123")]
  public void SigninRequestValidator_InvalidPasswordValues_ReturnsError(string? password)
  {
    // Arrange
    var model = new SigninRequest("example@email.com", password);
    var validator = new SigninRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.password).Only();
    ;
  }

  [Fact]
  public void SigninRequestValidator_InvalidPasswordLengthValues_ReturnsError()
  {
    // Arrange
    var longPassword = new string('a', 256 + 1);
    var model = new SigninRequest("example@email.com", longPassword);
    var validator = new SigninRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.password).Only();
    ;
  }
}
