using FluentAssertions;
using Moq;
using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services;
using Server.API.Services.Interfaces;

namespace Server.UnitTests.Services;

public class ReviewServiceTest
{
  [Fact]
  public async Task GetReviewByUsernameAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockReviewRepository
      .Setup(rr =>
        rr.GetReviewByUsernameAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()).Result
      )
      .Returns(
        new PaginatedListEnvelope<ReviewResponse>(
          [
            new ReviewResponse(
              0,
              new BookListResponse(0, "", "", null, null, "", null),
              "",
              DateTime.Now,
              null
            ),
          ],
          1,
          1,
          1
        )
      );

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await reviewService.GetReviewByUsernameAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
    response.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetReviewByUsernameAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(false);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.GetReviewByUsernameAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetReviewByBookAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var stubUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockReviewRepository
      .Setup(rr =>
        rr.GetReviewByBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()).Result
      )
      .Returns(
        new PaginatedListEnvelope<ReviewBookResponse>(
          [
            new ReviewBookResponse(
              0,
              new ReviewBookUserResponse("", "", null),
              "",
              DateTime.Now,
              null
            ),
          ],
          1,
          1,
          1
        )
      );

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      stubUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await reviewService.GetReviewByBookAsync(1, 1, 0);

    // Assert
    response.Should().NotBeNull();
    response.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetReviewByBookAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var stubUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(false);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      stubUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.GetReviewByBookAsync(1, 1, 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddReviewByBookAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var username = "username";
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUserInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockReviewRepository
      .Setup(rr => rr.AddReviewAsync(It.IsAny<Review>()).Result)
      .Returns(new Review() { User = new User() { Username = username } });

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await reviewService.AddReviewByBookAsync(1, 1, "");

    // Assert
    mockReviewRepository.Verify(rr => rr.AddReviewAsync(It.IsAny<Review>()), Times.Once());
    response.Should().Be(username);
  }

  [Fact]
  public async Task AddReviewByBookAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUserInDatabaseAsync(It.IsAny<int>()).Result).Returns(false);

    var reviewService = new ReviewService(
      stubReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.AddReviewByBookAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task AddReviewByBookAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUserInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(false);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.AddReviewByBookAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddReviewByBookAsync_ErrorSavingReview_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUserInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockReviewRepository
      .Setup(rr => rr.AddReviewAsync(It.IsAny<Review>()).Result)
      .Returns(() => null);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.AddReviewByBookAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task RemoveReviewAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockReviewRepository
      .Setup(rr => rr.RemoveReviewAsync(It.IsAny<int>()).Result)
      .Returns(new Review());

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    await reviewService.RemoveReviewAsync(1, "", 1);

    // Assert
    mockReviewRepository.Verify(rr => rr.RemoveReviewAsync(It.IsAny<int>()), Times.Once());
  }

  [Fact]
  public async Task RemoveReviewAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.RemoveReviewAsync(1, "", 1);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RemoveReviewAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.RemoveReviewAsync(1, "", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task RemoveReviewAsync_UserDoesntMatchUsername_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.RemoveReviewAsync(1, "", 1);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RemoveReviewAsync_NoReviewFound_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockReviewRepository.Setup(rr => rr.RemoveReviewAsync(It.IsAny<int>()).Result);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.RemoveReviewAsync(1, "", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task PatchReviewByIdAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var username = "username";
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = username });

    mockReviewRepository
      .Setup(rr => rr.GetReviewByIdAsync(It.IsAny<int>()).Result)
      .Returns(
        new ReviewBookResponse(
          1,
          new ReviewBookUserResponse(username, "", ""),
          "",
          DateTime.Now,
          null
        )
      );

    mockReviewRepository
      .Setup(rr => rr.PatchReviewAsync(It.IsAny<int>(), It.IsAny<string>()).Result)
      .Returns(
        new ReviewBookResponse(
          1,
          new ReviewBookUserResponse(username, "", ""),
          "",
          DateTime.Now,
          null
        )
      );

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await reviewService.PatchReviewByIdAsync(1, 1, "");

    // Assert
    mockReviewRepository.Verify(
      rr => rr.PatchReviewAsync(It.IsAny<int>(), It.IsAny<string>()),
      Times.Once()
    );
    response.Should().Be(username);
  }

  [Fact]
  public async Task PatchReviewByIdAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUserInDatabaseAsync(It.IsAny<int>()).Result);

    var reviewService = new ReviewService(
      stubReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.PatchReviewByIdAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task PatchReviewByIdAsync_InvalidReviewId_ThrowsError()
  {
    // Arrange
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result).Returns(new User());

    mockReviewRepository.Setup(rr => rr.GetReviewByIdAsync(It.IsAny<int>()).Result);

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.PatchReviewByIdAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task PatchReviewByIdAsync_EditAnotherUserReview_ThrowsError()
  {
    // Arrange
    var username = "username";
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result).Returns(new User());

    mockReviewRepository
      .Setup(rr => rr.GetReviewByIdAsync(It.IsAny<int>()).Result)
      .Returns(
        new ReviewBookResponse(
          1,
          new ReviewBookUserResponse(username, "", ""),
          "",
          DateTime.Now,
          null
        )
      );

    mockReviewRepository
      .Setup(rr => rr.PatchReviewAsync(It.IsAny<int>(), It.IsAny<string>()).Result)
      .Returns(
        new ReviewBookResponse(
          1,
          new ReviewBookUserResponse("some-other-user", "", ""),
          "",
          DateTime.Now,
          null
        )
      );

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.PatchReviewByIdAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task PatchReviewByIdAsync_ErrorPatchingReview_ThrowsError()
  {
    // Arrange
    var username = "username";
    var mockReviewRepository = new Mock<IReviewRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result).Returns(new User());

    mockReviewRepository
      .Setup(rr => rr.GetReviewByIdAsync(It.IsAny<int>()).Result)
      .Returns(
        new ReviewBookResponse(
          1,
          new ReviewBookUserResponse(username, "", ""),
          "",
          DateTime.Now,
          null
        )
      );

    mockReviewRepository.Setup(rr =>
      rr.PatchReviewAsync(It.IsAny<int>(), It.IsAny<string>()).Result
    );

    var reviewService = new ReviewService(
      mockReviewRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await reviewService.PatchReviewByIdAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }
}
