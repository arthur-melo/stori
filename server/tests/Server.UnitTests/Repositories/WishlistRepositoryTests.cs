using FluentAssertions;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class WishlistRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetWishlistByUsernameAsync_ValidUsername_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";

    var wishlist = new Wishlist() { UserId = 1, BookId = 1 };
    _context.Wishlists.Add(wishlist);

    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = username,
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var wishlistRepository = new WishlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await wishlistRepository.GetWishlistByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.book.id == 1).Should().NotBeNull();
  }

  [Fact]
  public async Task GetWishlistByUsernameAsync_InvalidUsername_ReturnsEmptyResponse()
  {
    // Arrange
    var username = "test";

    var wishlist = new Wishlist() { UserId = 1, BookId = 1 };
    _context.Wishlists.Add(wishlist);

    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "some-user",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var wishlistRepository = new WishlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await wishlistRepository.GetWishlistByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task GetWishlistByUsernameAndBookAsync_ValidData_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";
    var bookId = 1;

    var userRating = new Wishlist() { UserId = 1, BookId = bookId };
    _context.Wishlists.Add(userRating);

    var book = new Book()
    {
      Id = bookId,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
      Rating = new Rating(),
      PublishDate = new DateOnly(),
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = username,
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRatingRepository = new WishlistRepository(_context, autoMapper.mapper);

    var userRatingByBookResponse = autoMapper
      .mapper.ProjectTo<WishlistByBookResponse>(QueryableUtils.MapToIQueryable(userRating))
      .FirstOrDefault();

    // Act
    var actual = await userRatingRepository.GetWishlistByUsernameAndBookAsync(username, bookId);

    // Assert
    actual.Should().NotBeNull();
    actual!.Data.FirstOrDefault().Should().Be(userRatingByBookResponse);
  }

  [Fact]
  public async Task GetWishlistByUsernameAndBookAsync_InvalidData_ReturnsNull()
  {
    // Arrange
    var username = "test";
    var bookId = 1;

    var book = new Book()
    {
      Id = bookId,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
      Rating = new Rating(),
      PublishDate = new DateOnly(),
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = username,
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRatingRepository = new WishlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRatingRepository.GetWishlistByUsernameAndBookAsync(username, bookId);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task AddWishlistAsync_ValidWishlist_ReturnsValidResponse()
  {
    // Arrange
    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "some-user",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var wishlistRepository = new WishlistRepository(_context, autoMapper.mapper);

    var wishlist = new Wishlist() { UserId = 1, BookId = 1 };

    // Act
    var actual = await wishlistRepository.AddWishlistAsync(wishlist);

    // Assert
    actual.Should().NotBeNull();
    actual.Should().Be(wishlist);
  }

  [Fact]
  public async Task AddWishlistAsync_DuplicatedWishlist_ReturnsInvalidResponse()
  {
    // Arrange
    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "some-user",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var wishlist = new Wishlist() { UserId = 1, BookId = 1 };
    _context.Wishlists.Add(wishlist);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var wishlistRepository = new WishlistRepository(_context, autoMapper.mapper);

    var newWishlist = new Wishlist() { UserId = 1, BookId = 1 };

    // Act
    var actual = await wishlistRepository.AddWishlistAsync(newWishlist);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task RemoveWishlistAsync_RemoveWishlist_ReturnsValidResponse()
  {
    // Arrange
    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "some-user",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var wishlist = new Wishlist() { UserId = 1, BookId = 1 };
    _context.Wishlists.Add(wishlist);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var wishlistRepository = new WishlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await wishlistRepository.RemoveWishlistAsync(user.Id, book.Id);

    // Assert
    actual.Should().NotBeNull();
    _context.Wishlists.Should().HaveCount(0);
  }

  [Theory]
  [InlineData(1, 2)]
  [InlineData(2, 1)]
  [InlineData(2, 2)]
  public async Task RemoveWishlistAsync_InvalidParams_ReturnsInvalidResponse(int userId, int bookId)
  {
    // Arrange
    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "some-user",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var wishlist = new Wishlist() { UserId = 1, BookId = 1 };
    _context.Wishlists.Add(wishlist);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var wishlistRepository = new WishlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await wishlistRepository.RemoveWishlistAsync(userId, bookId);

    // Assert
    actual.Should().BeNull();
  }
}
