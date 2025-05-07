using FluentAssertions;
using Server.API.Repositories;
using Server.IntegrationTests.Helpers;
using SixLabors.ImageSharp;

namespace Server.IntegrationTests.Repositories;

public class ImageRepositoryTests
{
  [Fact]
  public async Task AddImageAsync_AddImage_ReturnsValidResponse()
  {
    // Arrange
    var imageInfo = new FileInfo(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );
    var image = Image.Load(imageInfo.OpenRead());

    var imageRepository = new ImageRepository();

    // Act
    await imageRepository.AddImageAsync(
      image,
      Path.Combine(Path.GetTempPath(), Constants.TEST_IMAGE_NAME)
    );

    var writtenImgInfo = new FileInfo(Path.Combine(Path.GetTempPath(), Constants.TEST_IMAGE_NAME));
    var writtenImage = Image.Load(imageInfo.OpenRead());

    // Assert
    writtenImage.Should().NotBeNull();

    // Cleanup
    File.Delete(writtenImgInfo.FullName);
  }

  [Fact]
  public async Task DeleteImage_ValidImage_ReturnsValidResponse()
  {
    // Arrange
    var imageInfo = new FileInfo(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );
    var image = Image.Load(imageInfo.OpenRead());

    var imageRepository = new ImageRepository();

    // Act
    await imageRepository.AddImageAsync(
      image,
      Path.Combine(Path.GetTempPath(), Constants.TEST_IMAGE_NAME)
    );

    var writtenImgInfo = new FileInfo(Path.Combine(Path.GetTempPath(), Constants.TEST_IMAGE_NAME));
    var writtenImage = Image.Load(imageInfo.OpenRead());

    // Assert
    writtenImage.Should().NotBeNull();

    imageRepository.DeleteImage(writtenImgInfo.FullName);

    var act = () => Image.Load(writtenImgInfo.OpenRead());

    act.Should().Throw<FileNotFoundException>();
  }

  [Fact]
  public void CreateDirectory_ValidPath_ReturnsValidResponse()
  {
    // Arrange
    var randomDirPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    var imageRepository = new ImageRepository();

    // Act
    imageRepository.CreateDirectory(randomDirPath);

    // Assert
    Directory.Exists(randomDirPath).Should().Be(true);

    // Cleanup
    Directory.Delete(randomDirPath);
    Directory.Exists(randomDirPath).Should().Be(false);
  }

  [Fact]
  public void isValidDirectory_ValidPath_ReturnsValidResponse()
  {
    // Arrange
    var imageRepository = new ImageRepository();

    // Act
    var isValidDirectory = imageRepository.IsValidDirectory(Path.GetTempPath());

    // Assert
    isValidDirectory.Should().Be(true);
  }

  [Fact]
  public void isValidDirectory_InvalidPath_ReturnsValidResponse()
  {
    // Arrange
    var imageRepository = new ImageRepository();

    // Act
    var isValidDirectory = imageRepository.IsValidDirectory("invalid");

    // Assert
    isValidDirectory.Should().Be(false);
  }
}
