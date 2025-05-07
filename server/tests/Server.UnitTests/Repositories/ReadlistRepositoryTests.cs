using FluentAssertions;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class ReadlistRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetReadlistByUsernameAsync_ValidUsername_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";

    var readlist = new Readlist() { UserId = 1, BookId = 1 };
    _context.Readlists.Add(readlist);

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
    var readlistRepository = new ReadlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await readlistRepository.GetReadlistByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.book.id == 1).Should().NotBeNull();
  }

  [Fact]
  public async Task GetReadlistByUsernameAsync_InvalidUsername_ReturnsEmptyResponse()
  {
    // Arrange
    var username = "test";

    var readlist = new Readlist() { UserId = 1, BookId = 1 };
    _context.Readlists.Add(readlist);

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
    var readlistRepository = new ReadlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await readlistRepository.GetReadlistByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task GetReadlistByUsernameAndBookAsync_ValidData_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";
    var bookId = 1;

    var userRating = new Readlist() { UserId = 1, BookId = bookId };
    _context.Readlists.Add(userRating);

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
    var userRatingRepository = new ReadlistRepository(_context, autoMapper.mapper);

    var userRatingByBookResponse = autoMapper
      .mapper.ProjectTo<ReadlistByBookResponse>(QueryableUtils.MapToIQueryable(userRating))
      .FirstOrDefault();

    // Act
    var actual = await userRatingRepository.GetReadlistByUsernameAndBookAsync(username, bookId);

    // Assert
    actual.Should().NotBeNull();
    actual!.Data.FirstOrDefault().Should().Be(userRatingByBookResponse);
  }

  [Fact]
  public async Task GetReadlistByUsernameAndBookAsync_InvalidData_ReturnsNull()
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
    var userRatingRepository = new ReadlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRatingRepository.GetReadlistByUsernameAndBookAsync(username, bookId);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task AddReadlistAsync_ValidReadlist_ReturnsValidResponse()
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
    var readlistRepository = new ReadlistRepository(_context, autoMapper.mapper);

    var readlist = new Readlist() { UserId = 1, BookId = 1 };

    // Act
    var actual = await readlistRepository.AddReadlistAsync(readlist);

    // Assert
    actual.Should().NotBeNull();
    actual.Should().Be(readlist);
  }

  [Fact]
  public async Task AddReadlistAsync_DuplicatedReadlist_ReturnsInvalidResponse()
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

    var readlist = new Readlist() { UserId = 1, BookId = 1 };
    _context.Readlists.Add(readlist);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var readlistRepository = new ReadlistRepository(_context, autoMapper.mapper);

    var newReadlist = new Readlist() { UserId = 1, BookId = 1 };

    // Act
    var actual = await readlistRepository.AddReadlistAsync(newReadlist);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task RemoveReadlistAsync_RemoveReadlist_ReturnsValidResponse()
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

    var readlist = new Readlist() { UserId = 1, BookId = 1 };
    _context.Readlists.Add(readlist);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var readlistRepository = new ReadlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await readlistRepository.RemoveReadlistAsync(user.Id, book.Id);

    // Assert
    actual.Should().NotBeNull();
    _context.Readlists.Should().HaveCount(0);
  }

  [Theory]
  [InlineData(1, 2)]
  [InlineData(2, 1)]
  [InlineData(2, 2)]
  public async Task RemoveReadlistAsync_InvalidParams_ReturnsInvalidResponse(int userId, int bookId)
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

    var readlist = new Readlist() { UserId = 1, BookId = 1 };
    _context.Readlists.Add(readlist);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var readlistRepository = new ReadlistRepository(_context, autoMapper.mapper);

    // Act
    var actual = await readlistRepository.RemoveReadlistAsync(userId, bookId);

    // Assert
    actual.Should().BeNull();
  }
}
