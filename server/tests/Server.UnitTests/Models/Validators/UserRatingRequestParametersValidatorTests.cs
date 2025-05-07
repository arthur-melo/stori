using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class UserRatingRequestParametersValidatorTests
{
  [Theory]
  [InlineData("aaa")]
  [InlineData("username")]
  [InlineData("username_valid")]
  [InlineData("username-valid")]
  [InlineData("0123456789_is-valid")]
  [InlineData("valid_username_length_32_charact")]
  public void UserRatingRequestParametersValidator_AllowedUsernamerParameter_ReturnsNoError(
    string username
  )
  {
    // Arrange
    var model = new UserRatingRequestParams(username, 1);
    var validator = new UserRatingRequestParametersValidator();

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
  public void UserRatingRequestParametersValidator_InvalidUsernameValues_ReturnsError(
    string username
  )
  {
    // Arrange
    var model = new UserRatingRequestParams(username, 1);
    var validator = new UserRatingRequestParametersValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.username).Only();
    ;
  }

  [Fact]
  public void UserRatingRequestParametersValidator_ValidBookIdValues_ReturnsNoError()
  {
    // Arrange
    var model = new UserRatingRequestParams("aaa", 1);
    var validator = new UserRatingRequestParametersValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void UserRatingRequestParametersValidator_InvalidBookIdValues_ReturnsError()
  {
    // Arrange
    var model = new UserRatingRequestParams("aaa", null);
    var validator = new UserRatingRequestParametersValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result
      .ShouldHaveValidationErrorFor(obj => obj.bookId)
      .Only()
      .WithErrorMessage($"bookId must not be null.");
    ;
  }
}
