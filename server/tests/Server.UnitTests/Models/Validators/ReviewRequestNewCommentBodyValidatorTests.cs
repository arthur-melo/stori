using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class ReviewRequestNewCommentBodyValidatorTests
{
  [Fact]
  public void ReviewRequestNewCommentBodyValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new ReviewRequestNewCommentBody("a");
    var validator = new ReviewRequestNewCommentBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData("")]
  [InlineData(null)]
  public void ReviewRequestNewCommentBodyValidator_InvalidTextValues_ReturnsError(string? text)
  {
    // Arrange
    var model = new ReviewRequestNewCommentBody(text);
    var validator = new ReviewRequestNewCommentBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.text).Only();
    ;
  }

  [Fact]
  public void ReviewRequestNewCommentBodyValidator_InvalidTextLengthValues_ReturnsError()
  {
    // Arrange
    var longText = new string('a', 1024 + 1);
    var model = new ReviewRequestNewCommentBody(longText);
    var validator = new ReviewRequestNewCommentBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.text).Only();
    ;
  }
}
