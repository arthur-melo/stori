using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class ReviewRequestCommentBodyValidatorTests
{
  [Fact]
  public void ReviewRequestCommentBodyValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new ReviewRequestCommentBody(1);
    var validator = new ReviewRequestCommentBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void ReviewRequestCommentBodyValidator_CommentIdValues_ReturnsError()
  {
    // Arrange
    var model = new ReviewRequestCommentBody(null);
    var validator = new ReviewRequestCommentBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.reviewId).Only();
    ;
  }
}
