using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class ReviewRequestBookParamsValidatorTests
{
  [Fact]
  public void ReviewRequestBookParamsValidator_ValidBookIdValues_ReturnsNoError()
  {
    // Arrange
    var model = new ReviewRequestBookParams(1);
    var validator = new ReviewRequestBookParamsValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void ReviewRequestBookParamsValidator_InvalidBookIdValues_ReturnsError()
  {
    // Arrange
    var model = new ReviewRequestBookParams(null);
    var validator = new ReviewRequestBookParamsValidator();

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
