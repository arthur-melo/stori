using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class UserPatchRequestBodyValidatorTests
{
  [Fact]
  public void UserPatchRequestBodyValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new UserPatchRequestBody("example@email.com", "password", "username", "name");
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData("invalid-email.com")]
  [InlineData("invalid")]
  [InlineData("@invalid.com")]
  public void UserPatchRequestBodyValidator_InvalidEmailValues_ReturnsError(string? email)
  {
    // Arrange
    var model = new UserPatchRequestBody(email, "password", "username", "name");
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.email).Only();
    ;
  }

  [Fact]
  public void UserPatchRequestBodyValidator_InvalidEmailLengthValues_ReturnsError()
  {
    // Arrange
    // Total: 257 characters for email, limit is 256.
    var longEmail = string.Concat(new string('a', 247), "@email.com");
    var model = new UserPatchRequestBody(longEmail, "password", "username", "name");
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.email).Only();
    ;
  }

  [Theory]
  [InlineData("    ")]
  [InlineData("123")]
  public void UserPatchRequestBodyValidator_InvalidPasswordValues_ReturnsError(string? password)
  {
    // Arrange
    var model = new UserPatchRequestBody("example@email.com", password, "username", "name");
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.password).Only();
    ;
  }

  [Fact]
  public void UserPatchRequestBodyValidator_InvalidPasswordLengthValues_ReturnsError()
  {
    // Arrange
    var longPassword = new string('a', 256 + 1);
    var model = new UserPatchRequestBody("example@email.com", longPassword, "username", "name");
    var validator = new UserPatchRequestBodyValidator();

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
    var model = new UserPatchRequestBody("example@email.com", "password", username, "name");
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
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
  public void UserPatchRequestBodyValidator_InvalidUsernameValues_ReturnsError(string username)
  {
    // Arrange
    var model = new UserPatchRequestBody("example@email.com", "password", username, "name");
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.username).Only();
    ;
  }

  [Theory]
  [InlineData(" ")]
  public void UserPatchRequestBodyValidator_InvalidNameValues_ReturnsError(string? name)
  {
    // Arrange
    var model = new UserPatchRequestBody("example@email.com", "password", "username", name);
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.name).Only();
    ;
  }

  [Fact]
  public void UserPatchRequestBodyValidator_InvalidNameLengthValue_ReturnsError()
  {
    // Arrange
    var longName = new string('a', 64 + 1);
    var model = new UserPatchRequestBody("example@email.com", "password", "username", longName);
    var validator = new UserPatchRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.name).Only();
    ;
  }
}
