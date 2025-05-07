using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class BookListRequestValidatorTests
{
  [Fact]
  public void BookListRequestValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new BookListRequest();
    var validator = new BookListRequestValidator();

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
  public void BookListRequestValidator_AllowedPageSizesValues_ReturnsNoError(int pageSize)
  {
    // Arrange
    var model = new BookListRequest(pageSize);
    var validator = new BookListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void BookListRequestValidator_AllowedPageNumberParameter_ReturnsNoError()
  {
    // Arrange
    var model = new BookListRequest(pageNumber: 1);
    var validator = new BookListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData("rating")]
  [InlineData("date")]
  public void BookListRequestValidator_AllowedOrderByValues_ReturnsNoError(string orderBy)
  {
    // Arrange
    var model = new BookListRequest(orderBy: orderBy);
    var validator = new BookListRequestValidator();

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
  public void BookListRequestValidator_InvalidPageSizesValues_ReturnsError(int pageSize)
  {
    // Arrange
    var model = new BookListRequest(pageSize);
    var validator = new BookListRequestValidator();

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
  public void BookListRequestValidator_InvalidPageNumberValues_ReturnsError(int pageNumber)
  {
    // Arrange
    var model = new BookListRequest(pageNumber: pageNumber);
    var validator = new BookListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result
      .ShouldHaveValidationErrorFor(obj => obj.pageNumber)
      .Only()
      .WithErrorMessage($"pageNumber must be greater than 0.");
    ;
  }

  [Theory]
  [InlineData("")]
  [InlineData("invalid")]
  public void BookListRequestValidator_InvalidOrderByValues_ReturnsError(string orderBy)
  {
    // Arrange
    var model = new BookListRequest(orderBy: orderBy);
    var validator = new BookListRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result
      .ShouldHaveValidationErrorFor(obj => obj.orderBy)
      .Only()
      .WithErrorMessage("orderBy must be one of the allowed values: rating, date.");
  }
}
