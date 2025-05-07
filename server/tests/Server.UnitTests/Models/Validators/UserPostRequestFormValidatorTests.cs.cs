using FileSignatures;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Validators;

namespace Server.UnitTests.Models.Validators;

public class UserPostRequestFormValidatorTests
{
  private readonly string TEST_IMAGE_DIR = "Data";
  private readonly FileFormatInspector _fileFormatInspector = new FileFormatInspector();

  [Theory]
  [InlineData("test-image.png")]
  [InlineData("test-image.jpg")]
  public void UserPostRequestFormValidator_ValidImageSignature_ReturnsNoError(string filename)
  {
    // Arrange
    var imageInfo = new FileInfo(Path.Combine(TEST_IMAGE_DIR, filename));
    var stubFormFile = new FormFile(imageInfo.OpenRead(), 0, imageInfo.Length, "name", "fileName");
    var model = new UserPostRequestForm(stubFormFile);

    var validator = new UserPostRequestFormValidator(_fileFormatInspector);

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void UserPostRequestFormValidator_InvalidImageSignature_ReturnsError()
  {
    // Arrange
    var imageInfo = new FileInfo(Path.Combine(TEST_IMAGE_DIR, "test-image.gif"));
    var stubFormFile = new FormFile(imageInfo.OpenRead(), 0, imageInfo.Length, "name", "fileName");
    var model = new UserPostRequestForm(stubFormFile);

    var validator = new UserPostRequestFormValidator(_fileFormatInspector);

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.profileImg).Only();
  }

  [Fact]
  public void UserPostRequestFormValidator_LargeImageSize_ReturnsError()
  {
    // Arrange
    // 2 MB + 1 byte
    var largeImageSize = (2 * 1024 * 1024) + 1;
    var imageInfo = new FileInfo(Path.Combine(TEST_IMAGE_DIR, "test-image.png"));
    var stubFormFile = new FormFile(imageInfo.OpenRead(), 0, largeImageSize, "name", "fileName");
    var model = new UserPostRequestForm(stubFormFile);

    var validator = new UserPostRequestFormValidator(_fileFormatInspector);

    // Act
    var result = validator.TestValidate(model);

    // Assert
    result.ShouldHaveValidationErrorFor(obj => obj.profileImg!.Length).Only();
  }
}
