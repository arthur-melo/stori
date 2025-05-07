using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class RefreshTokenValidatorRequestTests
{
  [Fact]
  public void RefreshTokenValidatorRequest_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new RefreshTokenRequest("a");
    var validator = new RefreshTokenValidatorRequest();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData("")]
  [InlineData(null)]
  public void RefreshTokenValidatorRequest_InvalidTokenValues_ReturnsError(string? refreshToken)
  {
    // Arrange
    var model = new RefreshTokenRequest(refreshToken);
    var validator = new RefreshTokenValidatorRequest();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.token).Only();
    ;
  }
}
