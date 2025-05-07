using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class PaginatedBookRequestValidatorTests
{
  [Fact]
  public void PaginatedBookRequestValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new PaginatedBookRequest(1);
    var validator = new PaginatedBookRequestValidator();

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
  public void PaginatedBookRequestValidator_AllowedPageSizesValues_ReturnsNoError(int pageSize)
  {
    // Arrange
    var model = new PaginatedBookRequest(1, pageSize);
    var validator = new PaginatedBookRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void PaginatedBookRequestValidator_AllowedPageNumberParameter_ReturnsNoError()
  {
    // Arrange
    var model = new PaginatedBookRequest(1, pageNumber: 1);
    var validator = new PaginatedBookRequestValidator();

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
  public void PaginatedBookRequestValidator_InvalidPageSizesValues_ReturnsError(int pageSize)
  {
    // Arrange
    var model = new PaginatedBookRequest(1, pageSize);
    var validator = new PaginatedBookRequestValidator();

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
  public void PaginatedBookRequestValidator_InvalidPageNumberValues_ReturnsError(int pageNumber)
  {
    // Arrange
    var model = new PaginatedBookRequest(1, pageNumber: pageNumber);
    var validator = new PaginatedBookRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result
      .ShouldHaveValidationErrorFor(obj => obj.pageNumber)
      .Only()
      .WithErrorMessage($"pageNumber must be greater than 0.");
    ;
  }

  [Fact]
  public void PaginatedBookRequestValidator_InvalidBookIdValues_ReturnsError()
  {
    // Arrange
    var model = new PaginatedBookRequest(null);
    var validator = new PaginatedBookRequestValidator();

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
