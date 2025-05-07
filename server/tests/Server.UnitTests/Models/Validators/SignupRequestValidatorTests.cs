using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class SignupRequestValidatorTests
{
  [Fact]
  public void SignupRequestValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new SignupRequest("username", "name", "example@email.com", "password");
    var validator = new SignupRequestValidator();

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
  public void SignupRequestValidator_InvalidEmailValues_ReturnsError(string? email)
  {
    // Arrange
    var model = new SignupRequest("username", "name", email, "password");
    var validator = new SignupRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.email).Only();
    ;
  }

  [Fact]
  public void SignupRequestValidator_InvalidEmailLengthValues_ReturnsError()
  {
    // Arrange
    // Total: 257 characters for email, limit is 256.
    var longEmail = string.Concat(new string('a', 247), "@email.com");
    var model = new SignupRequest("username", "name", longEmail, "password");
    var validator = new SignupRequestValidator();

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
  public void SignupRequestValidator_InvalidPasswordValues_ReturnsError(string? password)
  {
    // Arrange
    var model = new SignupRequest("username", "name", "example@email.com", password);
    var validator = new SignupRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.password).Only();
    ;
  }

  [Fact]
  public void SignupRequestValidator_InvalidPasswordLengthValues_ReturnsError()
  {
    // Arrange
    var longPassword = new string('a', 256 + 1);
    var model = new SignupRequest("username", "name", "example@email.com", longPassword);
    var validator = new SignupRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.password).Only();
    ;
  }

  [Theory]
  [InlineData("aaa")]
  [InlineData("username")]
  [InlineData("username_valid")]
  [InlineData("username-valid")]
  [InlineData("0123456789_is-valid")]
  [InlineData("valid_username_length_32_charact")]
  public void SigninRequestValidator_AllowedUsernamerParameter_ReturnsNoError(string username)
  {
    // Arrange
    var model = new SignupRequest(username, "name", "example@email.com", "password");
    var validator = new SignupRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData("")]
  [InlineData("aa")]
  [InlineData("Invalid")]
  [InlineData("invalid username with spaces")]
  [InlineData("invalid@symbol")]
  [InlineData("invalid_username_length_33_chars_")]
  [InlineData("_startswithInvalid")]
  [InlineData("-startswithInvalid")]
  [InlineData("endsWithInvalid_")]
  [InlineData("endsWithInvalid-")]
  [InlineData("not-_valid")]
  [InlineData("not_-valid")]
  public void SignupRequestValidator_InvalidUsernameValues_ReturnsError(string username)
  {
    // Arrange
    var model = new SignupRequest(username, "name", "example@email.com", "password");
    var validator = new SignupRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.username).Only();
    ;
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void SignupRequestValidator_InvalidNameValues_ReturnsError(string? name)
  {
    // Arrange
    var model = new SignupRequest("username", name, "example@email.com", "password");
    var validator = new SignupRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.name).Only();
    ;
  }

  [Fact]
  public void SignupRequestValidator_InvalidNameLengthValue_ReturnsError()
  {
    // Arrange
    var longName = new string('a', 64 + 1);
    var model = new SignupRequest("username", longName, "example@email.com", "password");
    var validator = new SignupRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.name).Only();
    ;
  }
}
