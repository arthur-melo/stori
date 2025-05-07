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

public class ReadlistServiceTests
{
  [Fact]
  public async Task GetReadlistAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockReadlistRepository
      .Setup(rr =>
        rr.GetReadlistByUsernameAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()).Result
      )
      .Returns(
        new PaginatedListEnvelope<ReadlistResponse>(
          [
            new ReadlistResponse(
              new BookListResponse(0, "", "", null, null, "", null),
              DateTime.Now
            ),
          ],
          1,
          1,
          1
        )
      );

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await readlistService.GetReadlistAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
    response.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetReadlistAsync_InvalidParameters_ThrowsError()
  {
    // Arrange
    var stubReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(false);

    var readlistService = new ReadlistService(
      stubReadlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.GetReadlistAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetReadlistByBookAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockReadlistRepository
      .Setup(urr =>
        urr.GetReadlistByUsernameAndBookAsync(It.IsAny<string>(), It.IsAny<int>()).Result
      )
      .Returns(new Envelope<ReadlistByBookResponse>([]));

    var userRatingService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await userRatingService.GetReadlistByBookAsync("", 1);

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetReadlistByBookAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result);

    var userRatingService = new ReadlistService(
      stubReadlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetReadlistByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetReadlistByBookAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var stubReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result);

    var userRatingService = new ReadlistService(
      stubReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetReadlistByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetReadlistByBookAsync_NoReadlist_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockReadlistRepository.Setup(urr =>
      urr.GetReadlistByUsernameAndBookAsync(It.IsAny<string>(), It.IsAny<int>()).Result
    );

    var userRatingService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetReadlistByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddReadlistAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockDateTimeService.Setup(dts => dts.Now()).Returns(DateTime.Now);

    mockReadlistRepository
      .Setup(rr => rr.AddReadlistAsync(It.IsAny<Readlist>()).Result)
      .Returns(new Readlist());

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    mockReadlistRepository.Verify(rr => rr.AddReadlistAsync(It.IsAny<Readlist>()), Times.Once());
  }

  [Fact]
  public async Task AddReadlistAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()));

    var readlistService = new ReadlistService(
      stubReadlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task AddReadlistAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var stubReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var readlistService = new ReadlistService(
      stubReadlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddReadlistAsync_NotMatchingUserAndUsername_ThrowsError()
  {
    // Arrange
    var stubReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var readlistService = new ReadlistService(
      stubReadlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task AddReadlistAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var stubReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(false);

    var readlistService = new ReadlistService(
      stubReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddReadlistAsync_ExistingReadlist_ReturnsValidResponse()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockDateTimeService.Setup(dts => dts.Now()).Returns(DateTime.Now);

    mockReadlistRepository
      .SetupSequence(rr => rr.AddReadlistAsync(It.IsAny<Readlist>()).Result)
      .Returns(() => null)
      .Returns(new Readlist() { BookId = 1, UserId = 1 });

    mockReadlistRepository
      .Setup(rr => rr.RemoveReadlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new Readlist());

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    mockReadlistRepository.Verify(
      rr => rr.AddReadlistAsync(It.IsAny<Readlist>()),
      Times.Exactly(2)
    );
  }

  [Fact]
  public async Task AddReadlistAsync_EmptyReadlistEntity_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockDateTimeService.Setup(dts => dts.Now()).Returns(DateTime.Now);

    mockReadlistRepository
      .SetupSequence(rr => rr.AddReadlistAsync(It.IsAny<Readlist>()).Result)
      .Returns(() => null)
      .Returns(new Readlist() { BookId = 1, UserId = 1 });

    mockReadlistRepository.Setup(rr =>
      rr.RemoveReadlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result
    );

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task AddReadlistAsync_ErrorReAddingReadlist_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var mockDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("username", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockDateTimeService.Setup(dts => dts.Now()).Returns(DateTime.Now);

    mockReadlistRepository
      .SetupSequence(rr => rr.AddReadlistAsync(It.IsAny<Readlist>()).Result)
      .Returns(() => null)
      .Returns(() => null);

    mockReadlistRepository
      .Setup(rr => rr.RemoveReadlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new Readlist());

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.AddReadlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task RemoveReadlistAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockReadlistRepository
      .Setup(rr => rr.RemoveReadlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new Readlist());

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    await readlistService.RemoveReadlistAsync(0, "username", 0);

    // Assert
    mockReadlistRepository.Verify(
      rr => rr.RemoveReadlistAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
  }

  [Fact]
  public async Task RemoveReadlistAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.RemoveReadlistAsync(0, "username", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task RemoveReadlistAsync_InvalidRequestedUsername_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.RemoveReadlistAsync(0, "invalid", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();

    mockUserRepository.Verify(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once());
  }

  [Fact]
  public async Task RemoveReadlistAsync_NotMatchingUserAndUsername_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.RemoveReadlistAsync(0, "invalid", 0);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RemoveReadlistAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(false);

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.RemoveReadlistAsync(0, "username", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    mockBookRepository.Verify(br => br.IsBookInDatabaseAsync(It.IsAny<int>()), Times.Once());
  }

  [Fact]
  public async Task RemoveReadlistAsync_ErrorRemovingReadlist_ThrowsError()
  {
    // Arrange
    var mockReadlistRepository = new Mock<IReadlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockReadlistRepository
      .Setup(rr => rr.RemoveReadlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(() => null);

    var readlistService = new ReadlistService(
      mockReadlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await readlistService.RemoveReadlistAsync(0, "username", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();

    mockReadlistRepository.Verify(
      rr => rr.RemoveReadlistAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
  }
}
