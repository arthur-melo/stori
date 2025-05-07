using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class PaginatedListRequestValidatorTests
{
  [Fact]
  public void PaginatedListRequestValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new PaginatedListRequest();
    var validator = new PaginatedListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData(10)]
  [InlineData(25)]
  [InlineData(50)]
  [InlineData(100)]
  public void PaginatedListRequestValidator_AllowedPageSizesValues_ReturnsNoError(int pageSize)
  {
    // Arrange
    var model = new PaginatedListRequest(pageSize);
    var validator = new PaginatedListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void PaginatedListRequestValidator_AllowedPageNumberParameter_ReturnsNoError()
  {
    // Arrange
    var model = new PaginatedListRequest(pageNumber: 1);
    var validator = new PaginatedListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  [InlineData(1)]
  [InlineData(101)]
  public void PaginatedListRequestValidator_InvalidPageSizesValues_ReturnsError(int pageSize)
  {
    // Arrange
    var model = new PaginatedListRequest(pageSize);
    var validator = new PaginatedListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result
      .ShouldHaveValidationErrorFor(obj => obj.pageSize)
      .Only()
      .WithErrorMessage($"pageSize must be one of the allowed values: 10, 25, 50, 100.");
    ;
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void PaginatedListRequestValidator_InvalidPageNumberValues_ReturnsError(int pageNumber)
  {
    // Arrange
    var model = new PaginatedListRequest(pageNumber: pageNumber);
    var validator = new PaginatedListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result
      .ShouldHaveValidationErrorFor(obj => obj.pageNumber)
      .Only()
      .WithErrorMessage($"pageNumber must be greater than 0.");
    ;
  }
}
