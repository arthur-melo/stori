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

public class WishlistServiceTests
{
  [Fact]
  public async Task GetWishlistAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockWishlistRepository
      .Setup(rr =>
        rr.GetWishlistByUsernameAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()).Result
      )
      .Returns(
        new PaginatedListEnvelope<WishlistResponse>(
          [
            new WishlistResponse(
              new BookListResponse(0, "", "", null, null, "", null),
              DateTime.Now
            ),
          ],
          1,
          1,
          1
        )
      );

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await wishlistService.GetWishlistAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
    response.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetWishlistAsync_InvalidParameters_ThrowsError()
  {
    // Arrange
    var stubWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(false);

    var wishlistService = new WishlistService(
      stubWishlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.GetWishlistAsync(1, 1, "");

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetWishlistByBookAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockWishlistRepository
      .Setup(urr =>
        urr.GetWishlistByUsernameAndBookAsync(It.IsAny<string>(), It.IsAny<int>()).Result
      )
      .Returns(new Envelope<WishlistByBookResponse>([]));

    var userRatingService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var response = await userRatingService.GetWishlistByBookAsync("", 1);

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetWishlistByBookAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result);

    var userRatingService = new WishlistService(
      stubWishlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetWishlistByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetWishlistByBookAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var stubWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result);

    var userRatingService = new WishlistService(
      stubWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetWishlistByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task GetWishlistByBookAsync_NoWishlist_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.IsUsernameInUseAsync(It.IsAny<string>()).Result)
      .Returns(true);

    mockBookRepository.Setup(br => br.IsBookInDatabaseAsync(It.IsAny<int>()).Result).Returns(true);

    mockWishlistRepository.Setup(urr =>
      urr.GetWishlistByUsernameAndBookAsync(It.IsAny<string>(), It.IsAny<int>()).Result
    );

    var userRatingService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await userRatingService.GetWishlistByBookAsync("", 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddWishlistAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
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

    mockWishlistRepository
      .Setup(ws => ws.AddWishlistAsync(It.IsAny<Wishlist>()).Result)
      .Returns(new Wishlist());

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    mockWishlistRepository.Verify(ws => ws.AddWishlistAsync(It.IsAny<Wishlist>()), Times.Once());
  }

  [Fact]
  public async Task AddWishlistAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var stubWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()));

    var wishlistService = new WishlistService(
      stubWishlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task AddWishlistAsync_InvalidUsername_ThrowsError()
  {
    // Arrange
    var stubWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var wishlistService = new WishlistService(
      stubWishlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddWishlistAsync_NotMatchingUserAndUsername_ThrowsError()
  {
    // Arrange
    var stubWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var stubBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "not-valid", null, DateTime.Now));

    var wishlistService = new WishlistService(
      stubWishlistRepository.Object,
      mockUserRepository.Object,
      stubBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task AddWishlistAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var stubWishlistRepository = new Mock<IWishlistRepository>();
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

    var wishlistService = new WishlistService(
      stubWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddWishlistAsync_ExistingWishlist_ReturnsValidResponse()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
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

    mockWishlistRepository
      .SetupSequence(ws => ws.AddWishlistAsync(It.IsAny<Wishlist>()).Result)
      .Returns(() => null)
      .Returns(new Wishlist() { BookId = 1, UserId = 1 });

    mockWishlistRepository
      .Setup(ws => ws.RemoveWishlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new Wishlist());

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    mockWishlistRepository.Verify(
      ws => ws.AddWishlistAsync(It.IsAny<Wishlist>()),
      Times.Exactly(2)
    );
  }

  [Fact]
  public async Task AddWishlistAsync_EmptyWishlistEntity_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
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

    mockWishlistRepository
      .SetupSequence(ws => ws.AddWishlistAsync(It.IsAny<Wishlist>()).Result)
      .Returns(() => null)
      .Returns(new Wishlist() { BookId = 1, UserId = 1 });

    mockWishlistRepository.Setup(rr =>
      rr.RemoveWishlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result
    );

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task AddWishlistAsync_ErrorReAddingWishlist_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
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

    mockWishlistRepository
      .SetupSequence(ws => ws.AddWishlistAsync(It.IsAny<Wishlist>()).Result)
      .Returns(() => null)
      .Returns(() => null);

    mockWishlistRepository
      .Setup(ws => ws.RemoveWishlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new Wishlist());

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      mockDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.AddWishlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task RemoveWishlistAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
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

    mockWishlistRepository
      .Setup(ws => ws.RemoveWishlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(new Wishlist());

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    await wishlistService.RemoveWishlistAsync(0, "username", 0);

    // Assert
    mockWishlistRepository.Verify(
      ws => ws.RemoveWishlistAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
  }

  [Fact]
  public async Task RemoveWishlistAsync_InvalidUser_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository.Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result);

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.RemoveWishlistAsync(0, "username", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task RemoveWishlistAsync_InvalidRequestedUsername_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "username" });

    mockUserRepository.Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result);

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.RemoveWishlistAsync(0, "invalid", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();

    mockUserRepository.Verify(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once());
  }

  [Fact]
  public async Task RemoveWishlistAsync_NotMatchingUserAndUsername_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
    var mockUserRepository = new Mock<IUserRepository>();
    var mockBookRepository = new Mock<IBookRepository>();
    var stubDateTimeService = new Mock<IDateTimeService>();

    mockUserRepository
      .Setup(ur => ur.GetUserByIdAsync(It.IsAny<int>()).Result)
      .Returns(new User() { Username = "invalid" });

    mockUserRepository
      .Setup(ur => ur.GetUserByUsernameAsync(It.IsAny<string>()).Result)
      .Returns(new UserUnauthorizedResponse("", "", null, DateTime.Now));

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.RemoveWishlistAsync(0, "", 0);

    // Assert
    await act.Should().ThrowAsync<ValidationException>();
  }

  [Fact]
  public async Task RemoveWishlistAsync_InvalidBook_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
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

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.RemoveWishlistAsync(0, "username", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();

    mockBookRepository.Verify(br => br.IsBookInDatabaseAsync(It.IsAny<int>()), Times.Once());
  }

  [Fact]
  public async Task RemoveWishlistAsync_ErrorRemovingWishlist_ThrowsError()
  {
    // Arrange
    var mockWishlistRepository = new Mock<IWishlistRepository>();
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

    mockWishlistRepository
      .Setup(wr => wr.RemoveWishlistAsync(It.IsAny<int>(), It.IsAny<int>()).Result)
      .Returns(() => null);

    var wishlistService = new WishlistService(
      mockWishlistRepository.Object,
      mockUserRepository.Object,
      mockBookRepository.Object,
      stubDateTimeService.Object
    );

    // Act
    var act = async () => await wishlistService.RemoveWishlistAsync(0, "username", 0);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();

    mockWishlistRepository.Verify(
      wr => wr.RemoveWishlistAsync(It.IsAny<int>(), It.IsAny<int>()),
      Times.Once()
    );
  }
}
