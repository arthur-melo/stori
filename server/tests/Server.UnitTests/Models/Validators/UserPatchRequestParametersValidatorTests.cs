using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class UserPatchRequestParametersValidatorTests
{
  [Theory]
  [InlineData("aaa")]
  [InlineData("username")]
  [InlineData("username_valid")]
  [InlineData("username-valid")]
  [InlineData("0123456789_is-valid")]
  [InlineData("valid_username_length_32_charact")]
  public void UserPatchRequestParametersValidator_AllowedUsernamerParameter_ReturnsNoError(
    string username
  )
  {
    // Arrange
    var model = new UserPatchRequestParameters(username);
    var validator = new UserPatchRequestParametersValidator();

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
  public void UserPatchRequestParametersValidator_InvalidUsernameValues_ReturnsError(
    string username
  )
  {
    // Arrange
    var model = new UserPatchRequestParameters(username);
    var validator = new UserPatchRequestParametersValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.username).Only();
    ;
  }
}
