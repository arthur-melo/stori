using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Server.API.Options;
using Server.API.Repositories;
using Server.API.Services;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Services;

public class ImageServiceTests
{
  [Fact]
  public async Task ProcessImageAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var fileUploadOptions = Options.Create(
      new FileUploadOptions() { Path = Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR) }
    );

    var imageInfo = new FileInfo(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );
    var formFile = new FormFile(imageInfo.OpenRead(), 0, imageInfo.Length, "name", "fileName");
    var imageRepository = new ImageRepository();

    var imageService = new ImageService(fileUploadOptions, imageRepository);

    // Act
    var response = await imageService.ProcessImageAsync(formFile);

    // Assert
    response.Should().NotBeNull();
    File.Exists(Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR, response)).Should().BeTrue();

    // Cleanup
    File.Delete(Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR, response));
    Directory.Delete(Constants.TEST_IMAGE_OUTPUT_DIR);
  }

  [Fact]
  public async Task DeleteImage_ValidParameters_RemovesFileFromSystem()
  {
    // Arrange
    var fileUploadOptions = Options.Create(
      new FileUploadOptions() { Path = Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR) }
    );

    var imageInfo = new FileInfo(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );
    var formFile = new FormFile(imageInfo.OpenRead(), 0, imageInfo.Length, "name", "fileName");
    var imageRepository = new ImageRepository();

    var imageService = new ImageService(fileUploadOptions, imageRepository);

    // Act
    // Create file first
    var response = await imageService.ProcessImageAsync(formFile);

    // Delete it
    imageService.DeleteImage(response);

    // Assert
    File.Exists(Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR, response)).Should().BeFalse();
  }

  [Fact]
  public async Task ValidateDirectory_ValidParameters_VerifiesDirectoryExists()
  {
    // Arrange
    var fileUploadOptions = Options.Create(
      new FileUploadOptions() { Path = Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR) }
    );

    var imageInfo = new FileInfo(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );
    var formFile = new FormFile(imageInfo.OpenRead(), 0, imageInfo.Length, "name", "fileName");
    var imageRepository = new ImageRepository();

    var imageService = new ImageService(fileUploadOptions, imageRepository);

    // Act
    var response = await imageService.ProcessImageAsync(formFile);

    // Assert
    imageService
      .ValidateDirectory(Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR))
      .Should()
      .BeTrue();

    // Cleanup
    File.Delete(Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR, response));
    Directory.Delete(Constants.TEST_IMAGE_OUTPUT_DIR);
  }

  [Fact]
  public void ValidateDirectory_InvalidParameters_VerifiesDirectoryDoesntExists()
  {
    // Arrange
    var fileUploadOptions = Options.Create(
      new FileUploadOptions() { Path = Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR) }
    );
    var imageRepository = new ImageRepository();

    var imageService = new ImageService(fileUploadOptions, imageRepository);

    // Act

    // Assert
    imageService
      .ValidateDirectory(Path.Combine(Constants.TEST_IMAGE_OUTPUT_DIR, "invalid"))
      .Should()
      .BeFalse();
  }
}
