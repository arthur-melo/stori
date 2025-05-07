using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class ReviewRequestEditParamsValidatorTests
{
  [Fact]
  public void ReviewRequestEditParamsValidator_ValidReviewIdValues_ReturnsNoError()
  {
    // Arrange
    var model = new ReviewRequestEditParams(1);
    var validator = new ReviewRequestEditParamsValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void ReviewRequestEditParamsValidator_InvalidReviewIdValues_ReturnsError()
  {
    // Arrange
    var model = new ReviewRequestEditParams(null);
    var validator = new ReviewRequestEditParamsValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result
      .ShouldHaveValidationErrorFor(obj => obj.reviewId)
      .Only()
      .WithErrorMessage($"reviewId must not be null.");
    ;
  }
}
