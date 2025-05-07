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

public class UserRatingServiceTests
{
  [Fact]
  public async Task GetUserRatingAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockUserRatingRepository
      .Setup(urr =>
        urr.GetUserRatingByUsernameAsync(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>()
        ).Result
      )
      .Returns(new PaginatedListEnvelope<UserRatingResponse>([], 0, 0, 0));

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await userRatingService.GetUserRatingAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetUserRatingAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(false);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetUserRatingAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddUserRatingAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository
      .Setup(urr => urr.AddUserRatingAsync(It.IsAny<UserRating>()).Result)
      .Returns(new UserRating());

    mockRatingService.Setup(rs => rs.AddBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()));

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    mockRatingService.Verify(
      rs => rs.AddBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
  }

  [Fact]
  public async Task GetUserRatingByBookAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository
      .Setup(urr =>
        urr.GetUserRatingByUsernameAndBookAsync(It.IsAny<string>(), It.IsAny<int>()).Result
      )
      .Returns(new Envelope<UserRatingByBookResponse>([]));

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await userRatingService.GetUserRatingByBookAsync("", 1);

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetUserRatingByBookAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetUserRatingByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetUserRatingByBookAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetUserRatingByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetUserRatingByBookAsync_NoUserRating_ThrowsError()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository.Setup(urr =>
      urr.GetUserRatingByUsernameAndBookAsync(It.IsAny<string>(), It.IsAny<int>()).Result
    );

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetUserRatingByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddUserRatingAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task AddUserRatingAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddUserRatingAsync_UserDontMatchWithUsername_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task AddUserRatingAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(false);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddUserRatingAsync_ExistingUserRating_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository
      .SetupSequence(urr => urr.AddUserRatingAsync(It.IsAny<UserRating>()).Result)
      .Returns(() => null)
      .Returns(new UserRating());

    mockRatingService.Setup(rs => rs.AddBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()));

    mockUserRatingRepository
      .Setup(urr => urr.RemoveUserRatingAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new UserRating());

    mockRatingService.Setup(rs => rs.DeleteBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()));

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    mockRatingService.Verify(
      rs => rs.AddBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
    mockUserRatingRepository.Verify(
      urr => urr.AddUserRatingAsync(It.IsAny<UserRating>()),
      Times.Exactly(2)
    );
  }

  [Fact]
  public async Task AddUserRatingAsync_RemoveUserRating_ThrowsError()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "username", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository
      .SetupSequence(urr => urr.AddUserRatingAsync(It.IsAny<UserRating>()).Result)
      .Returns(() => null)
      .Returns(new UserRating());

    mockRatingService.Setup(rs => rs.AddBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()));

    mockUserRatingRepository.Setup(urr =>
      urr.RemoveUserRatingAsync(It.IsAny<int>(), It.IsAny<int>()).Result
    );

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task AddUserRatingAsync_ExistingUserRatingErrorReAdding_ThrowsError()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "username", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository
      .SetupSequence(urr => urr.AddUserRatingAsync(It.IsAny<UserRating>()).Result)
      .Returns(() => null)
      .Returns(() => null);

    mockRatingService.Setup(rs => rs.AddBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()));

    mockUserRatingRepository
      .Setup(urr => urr.RemoveUserRatingAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new UserRating());

    mockRatingService.Setup(rs => rs.DeleteBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()));

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.AddUserRatingAsync(1, "", 1, 1);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task RemoveUserRatingAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository
      .Setup(urr => urr.RemoveUserRatingAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new UserRating());

    mockRatingService.Setup(rs => rs.DeleteBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()));

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    await userRatingService.RemoveUserRatingAsync(1, "username", 1);

    // Assert
    mockRatingService.Verify(
      rs => rs.DeleteBookRatingAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
  }

  [Fact]
  public async Task RemoveUserRatingAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.RemoveUserRatingAsync(1, "username", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task RemoveUserRatingAsync_InvalidRequestedUsername_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "invalid" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.RemoveUserRatingAsync(1, "username", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();

    mockUserRepository.Verify(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once());
  }

  [Fact]
  public async Task RemoveUserRatingAsync_UsernameDoesntMatchUser_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.RemoveUserRatingAsync(1, "", 1);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RemoveUserRatingAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var stubUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(false);

    var userRatingService = new UserRatingService(
      stubUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.RemoveUserRatingAsync(1, "username", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task RemoveUserRatingAsync_NoUserRatingFound_ThrowsError()
  {
    // Arrange
    var mockUserRatingRepository = new Mock<IUserRatingRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockRatingService = new Mock<IRatingService>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockUserRatingRepository.Setup(urr =>
      urr.RemoveUserRatingAsync(It.IsAny<int>(), It.IsAny<int>()).Result
    );

    var userRatingService = new UserRatingService(
      mockUserRatingRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockRatingService.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.RemoveUserRatingAsync(1, "username", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    mockUserRatingRepository.Verify(
      urr => urr.RemoveUserRatingAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
  }
}
