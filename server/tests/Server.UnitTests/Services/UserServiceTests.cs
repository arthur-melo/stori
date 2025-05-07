using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Options;
using Server.API.Repositories.Interfaces;
using Server.API.Services;
using Server.API.Services.Interfaces;

namespace Server.UnitTests.Services;

public class UserServiceTests
{
  [Fact]
  public async Task GetUserByIdAsync_WithUnauthorizedUser_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository.Setup(ur => ur.GetUserResponseByIdAsync(It.IsAny<int>()).Result);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.GetUserByIdAsync(1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetUserByIdAsync_WithAuthorizedUser_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserResponseByIdAsync(It.IsAny<int>()).Result)
      .Returns(new UserAuthorizedResponse("", "", "", null, DateTime.Now));

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var response = await userService.GetUserByIdAsync(1);

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetUserByUsernameAsync_ValidParameters_RequestToOtherUsername_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User { Id = 1, Username = "NotTheSameUser" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var response = await userService.GetUserByUsernameAsync("");

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetUserByUsernameAsync_InvalidUser_WithoutAuthorization_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.GetUserByUsernameAsync("");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetUserByUsernameAsync_InvalidUser_WithAuthorization_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.GetUserByUsernameAsync("");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task PatchUserAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ).Result
      )
      .Returns(
        new Envelope<UserAuthorizedResponse>(
          [new UserAuthorizedResponse("", "", "", null, DateTime.Now)]
        )
      );

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var response = await userService.PatchUserAsync(1, "username", null, null, null, null);

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task PatchUserAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.PatchUserAsync(1, "username", null, null, null, null);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task PatchUserAsync_UsernameDoesntMatchUser_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "invalid" });

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.PatchUserAsync(1, "username", null, null, null, null);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task PatchUserAsync_NewUsername_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ).Result
      )
      .Returns(
        new Envelope<UserAuthorizedResponse>(
          [new UserAuthorizedResponse("", "", "", null, DateTime.Now)]
        )
      );

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(false);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var response = await userService.PatchUserAsync(1, "username", null, null, "newUsername", null);

    // Assert
    response.Should().NotBeNull();
    mockUserRepository.Verify(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()), Times.Once());
  }

  [Fact]
  public async Task PatchUserAsync_NewUsernameInUse_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ).Result
      )
      .Returns(
        new Envelope<UserAuthorizedResponse>(
          [new UserAuthorizedResponse("", "", "", null, DateTime.Now)]
        )
      );

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () =>
      await userService.PatchUserAsync(1, "username", null, null, "newUsername", null);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task PatchUserAsync_NewEmail_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ).Result
      )
      .Returns(
        new Envelope<UserAuthorizedResponse>(
          [new UserAuthorizedResponse("", "", "", null, DateTime.Now)]
        )
      );

    mockUserRepository.Setup(ur => ur.IsEmailInUseAsync(It.IsAny<string>()).Result).Returns(false);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var response = await userService.PatchUserAsync(
      1,
      "username",
      "newemail@example.com",
      null,
      null,
      null
    );

    // Assert
    response.Should().NotBeNull();
    mockUserRepository.Verify(ur => ur.IsEmailInUseAsync(It.IsAny<string>()), Times.Once());
  }

  [Fact]
  public async Task PatchUserAsync_NewEmailInUse_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ).Result
      )
      .Returns(
        new Envelope<UserAuthorizedResponse>(
          [new UserAuthorizedResponse("", "", "", null, DateTime.Now)]
        )
      );

    mockUserRepository.Setup(ur => ur.IsEmailInUseAsync(It.IsAny<string>()).Result).Returns(true);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () =>
      await userService.PatchUserAsync(1, "username", "newemail@example.com", null, null, null);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task PatchUserAsync_NewPassword_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var mockEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ).Result
      )
      .Returns(
        new Envelope<UserAuthorizedResponse>(
          [new UserAuthorizedResponse("", "", "", null, DateTime.Now)]
        )
      );

    mockEncryptionService.Setup(es => es.HashPassword(It.IsAny<string>())).Returns("");

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      mockEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var response = await userService.PatchUserAsync(1, "username", null, "newpassword", null, null);

    // Assert
    response.Should().NotBeNull();
    mockEncryptionService.Verify(es => es.HashPassword(It.IsAny<string>()), Times.Once());
  }

  [Fact]
  public async Task PatchUserAsync_ErrorPatchingUserOnDatabase_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var mockEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur =>
      ur.PatchUserAsync(
        It.IsAny<int>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>()
      ).Result
    );

    mockEncryptionService.Setup(es => es.HashPassword(It.IsAny<string>())).Returns("");

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      mockEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.PatchUserAsync(1, "username", null, null, null, null);

    // Assert
    await act.Should().ThrowAsync<Exception>();
    mockUserRepository.Verify(
      ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ),
      Times.Once()
    );
  }

  [Fact]
  public async Task PostUserPhotoAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var mockImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockImageService
      .Setup(imgs => imgs.ProcessImageAsync(It.IsAny<IFormFile>()).Result)
      .Returns("");

    mockUserRepository.Setup(ur =>
      ur.PatchUserAsync(
        It.IsAny<int>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>()
      ).Result
    );

    var mockIFormFile = new Mock<IFormFile>();

    var userService = new UserService(
      mockUserRepository.Object,
      mockImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    await userService.PostUserPhotoAsync(1, "username", mockIFormFile.Object);

    // Assert
    mockUserRepository.Verify(
      ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ),
      Times.Once()
    );
  }

  [Fact]
  public async Task PostUserPhotoAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var mockIFormFile = new Mock<IFormFile>();

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.PostUserPhotoAsync(1, "username", mockIFormFile.Object);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task PostUserPhotoAsync_UsernameDoesntMatchUser_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "invalid" });

    var mockIFormFile = new Mock<IFormFile>();

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.PostUserPhotoAsync(1, "username", mockIFormFile.Object);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task PostUserPhotoAsync_InvalidDirectoryDeletePreviousUserImg_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var mockImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var mockFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username", ProfileImg = "img.png" });

    mockFileUploadOptions.Setup(fuo => fuo.Value).Returns(new FileUploadOptions() { Path = "" });
    mockImageService.Setup(imgs => imgs.DeleteImage(It.IsAny<string>()));

    mockImageService
      .Setup(imgs => imgs.ProcessImageAsync(It.IsAny<IFormFile>()).Result)
      .Returns("");

    mockImageService.Setup(imgs => imgs.ValidateDirectory(It.IsAny<string>())).Returns(false);

    mockUserRepository.Setup(ur =>
      ur.PatchUserAsync(
        It.IsAny<int>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>()
      ).Result
    );

    var mockIFormFile = new Mock<IFormFile>();

    var userService = new UserService(
      mockUserRepository.Object,
      mockImageService.Object,
      stubEncryptionService.Object,
      mockFileUploadOptions.Object
    );

    // Act
    await userService.PostUserPhotoAsync(1, "username", mockIFormFile.Object);

    // Assert
    mockImageService.Verify(imgs => imgs.ValidateDirectory(It.IsAny<string>()), Times.Once());
    mockImageService.Verify(imgs => imgs.DeleteImage(It.IsAny<string>()), Times.Never());
    mockUserRepository.Verify(
      ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ),
      Times.Once()
    );
  }

  [Fact]
  public async Task PostUserPhotoAsync_DeletePreviousUserImg_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var mockImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var mockFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username", ProfileImg = "img.png" });

    mockFileUploadOptions.Setup(fuo => fuo.Value).Returns(new FileUploadOptions() { Path = "" });
    mockImageService.Setup(imgs => imgs.ValidateDirectory(It.IsAny<string>())).Returns(true);
    mockImageService.Setup(imgs => imgs.DeleteImage(It.IsAny<string>()));

    mockImageService
      .Setup(imgs => imgs.ProcessImageAsync(It.IsAny<IFormFile>()).Result)
      .Returns("");

    mockUserRepository.Setup(ur =>
      ur.PatchUserAsync(
        It.IsAny<int>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>(),
        It.IsAny<string?>()
      ).Result
    );

    var mockIFormFile = new Mock<IFormFile>();

    var userService = new UserService(
      mockUserRepository.Object,
      mockImageService.Object,
      stubEncryptionService.Object,
      mockFileUploadOptions.Object
    );

    // Act
    await userService.PostUserPhotoAsync(1, "username", mockIFormFile.Object);

    // Assert
    mockImageService.Verify(imgs => imgs.DeleteImage(It.IsAny<string>()), Times.Once());
    mockUserRepository.Verify(
      ur =>
        ur.PatchUserAsync(
          It.IsAny<int>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ),
      Times.Once()
    );
  }

  [Fact]
  public async Task RemoveUserPhotoAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var mockImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var mockFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockUserRepository
      .Setup(ur => ur.RemoveUserPhotoAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockFileUploadOptions.Setup(fuo => fuo.Value).Returns(new FileUploadOptions() { Path = "" });
    mockImageService.Setup(ims => ims.ValidateDirectory(It.IsAny<string>())).Returns(true);

    var userService = new UserService(
      mockUserRepository.Object,
      mockImageService.Object,
      stubEncryptionService.Object,
      mockFileUploadOptions.Object
    );

    // Act
    await userService.RemoveUserPhotoAsync(1, "");

    // Assert
    mockUserRepository.Verify(ur => ur.RemoveUserPhotoAsync(It.IsAny<int>()), Times.Once());
  }

  [Fact]
  public async Task RemoveUserPhotoAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.RemoveUserPhotoAsync(1, "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RemoveUserPhotoAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.RemoveUserPhotoAsync(1, "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task RemoveUserPhotoAsync_UserDoesntMatchUsername_ThrowsError()
  {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    var stubImageService = new Mock<IImageService>();
    var stubEncryptionService = new Mock<IEncryptionService>();
    var stubFileUploadOptions = new Mock<IOptions<FileUploadOptions>>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var userService = new UserService(
      mockUserRepository.Object,
      stubImageService.Object,
      stubEncryptionService.Object,
      stubFileUploadOptions.Object
    );

    // Act
    var act = async () => await userService.RemoveUserPhotoAsync(1, "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }
}
