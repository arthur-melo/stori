using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class UserRatingRequestBodyValidatorTests
{
  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public void UserRatingRequestBodyValidator_DefaultParameters_ReturnsNoError(int rating)
  {
    // Arrange
    var model = new UserRatingRequestBody(rating);
    var validator = new UserRatingRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData(null)]
  [InlineData(0)]
  [InlineData(6)]
  public void UserRatingRequestBodyValidator_InvalidRating_ReturnsError(int? rating)
  {
    // Arrange
    var model = new UserRatingRequestBody(rating);
    var validator = new UserRatingRequestBodyValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.rating).Only();
  }
}
