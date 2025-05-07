using FluentValidation.TestHelper;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class PaginatedUserRequestValidatorTests
{
  [Fact]
  public void PaginatedUserRequestValidator_DefaultParameters_ReturnsNoError()
  {
    // Arrange
    var model = new PaginatedUserRequest("aaa");
    var validator = new PaginatedUserRequestValidator();

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
  public void PaginatedUserRequestValidator_AllowedPageSizesValues_ReturnsNoError(int pageSize)
  {
    // Arrange
    var model = new PaginatedUserRequest("aaa", pageSize);
    var validator = new PaginatedUserRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void PaginatedUserRequestValidator_AllowedPageNumberParameter_ReturnsNoError()
  {
    // Arrange
    var model = new PaginatedUserRequest("aaa", pageNumber: 1);
    var validator = new PaginatedUserRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Theory]
  [InlineData("aaa")]
  [InlineData("username")]
  [InlineData("username_valid")]
  [InlineData("username-valid")]
  [InlineData("0123456789_is-valid")]
  [InlineData("valid_username_length_32_charact")]
  public void PaginatedUserRequestValidator_AllowedUsernamerParameter_ReturnsNoError(
    string username
  )
  {
    // Arrange
    var model = new PaginatedUserRequest(username);
    var validator = new PaginatedUserRequestValidator();

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
  public void PaginatedUserRequestValidator_InvalidPageSizesValues_ReturnsError(int pageSize)
  {
    // Arrange
    var model = new PaginatedUserRequest("aaa", pageSize);
    var validator = new PaginatedUserRequestValidator();

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
  public void PaginatedUserRequestValidator_InvalidPageNumberValues_ReturnsError(int pageNumber)
  {
    // Arrange
    var model = new PaginatedUserRequest("aaa", pageNumber: pageNumber);
    var validator = new PaginatedUserRequestValidator();

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
  public void PaginatedUserRequestValidator_InvalidUsernameValues_ReturnsError(string username)
  {
    // Arrange
    var model = new PaginatedUserRequest(username);
    var validator = new PaginatedUserRequestValidator();

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.username).Only();
    ;
  }
}
