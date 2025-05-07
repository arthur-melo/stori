using FluentAssertions;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class UserRatingRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetUserRatingByUsernameAsync_ValidUsername_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";

    var userRating = new UserRating()
    {
      UserId = 1,
      BookId = 1,
      Rating = 1,
    };
    _context.UserRatings.Add(userRating);

    var book = new Book()
    {
      Id = 1,
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
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    var userRatingResponse = autoMapper
      .mapper.ProjectTo<UserRatingResponse>(QueryableUtils.MapToIQueryable(userRating))
      .FirstOrDefault();

    // Act
    var actual = await userRatingRepository.GetUserRatingByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.book.id == book.Id).Should().Be(userRatingResponse);
  }

  [Fact]
  public async Task GetUserRatingByUsernameAsync_InvalidUsername_ReturnsEmptyResponse()
  {
    // Arrange
    var username = "test";

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRatingRepository.GetUserRatingByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task GetUserRatingByUsernameAndBookAsync_ValidData_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";
    var bookId = 1;

    var userRating = new UserRating()
    {
      UserId = 1,
      BookId = bookId,
      Rating = 1,
    };
    _context.UserRatings.Add(userRating);

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
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    var userRatingByBookResponse = autoMapper
      .mapper.ProjectTo<UserRatingByBookResponse>(QueryableUtils.MapToIQueryable(userRating))
      .FirstOrDefault();

    // Act
    var actual = await userRatingRepository.GetUserRatingByUsernameAndBookAsync(username, bookId);

    // Assert
    actual.Should().NotBeNull();
    actual!.Data.FirstOrDefault().Should().Be(userRatingByBookResponse);
  }

  [Fact]
  public async Task GetUserRatingByUsernameAndBookAsync_InvalidData_ReturnsNull()
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
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRatingRepository.GetUserRatingByUsernameAndBookAsync(username, bookId);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task AddUserRatingAsync_ValidUserRating_ReturnsValidResponse()
  {
    // Arrange
    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
      PublishDate = new DateOnly(),
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    var userRating = new UserRating()
    {
      UserId = 1,
      BookId = 1,
      Rating = 1,
    };

    // Act
    var actual = await userRatingRepository.AddUserRatingAsync(userRating);

    // Assert
    actual.Should().NotBeNull();
    actual.Should().Be(userRating);
    _context.UserRatings.Should().HaveCount(1);
  }

  [Fact]
  public async Task AddUserRatingAsync_DuplicatedUserRating_ReturnsNullResponse()
  {
    // Arrange
    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
      PublishDate = new DateOnly(),
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var userRating = new UserRating()
    {
      UserId = 1,
      BookId = 1,
      Rating = 1,
    };

    _context.UserRatings.Add(userRating);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    var duplicateUserRating = new UserRating()
    {
      UserId = 1,
      BookId = 1,
      Rating = 1,
    };

    // Act
    var actual = await userRatingRepository.AddUserRatingAsync(duplicateUserRating);

    // Assert
    actual.Should().BeNull();
    _context.UserRatings.Should().HaveCount(1);
  }

  [Fact]
  public async Task RemoveUserRatingAsync_ValidUserRating_ReturnsValidResponse()
  {
    // Arrange
    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
      PublishDate = new DateOnly(),
    };
    _context.Books.Add(book);

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var userRating = new UserRating()
    {
      UserId = 1,
      BookId = 1,
      Rating = 1,
    };

    _context.UserRatings.Add(userRating);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRatingRepository.RemoveUserRatingAsync(user.Id, book.Id);

    // Assert
    actual.Should().NotBeNull();
    actual.Should().Be(userRating);
    _context.UserRatings.Should().HaveCount(0);
  }

  [Fact]
  public async Task RemoveUserRatingAsync_InvalidUserRating_ReturnsNullResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRatingRepository = new UserRatingRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRatingRepository.RemoveUserRatingAsync(1, 1);

    // Assert
    actual.Should().BeNull();
  }
}
