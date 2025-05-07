using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class ReviewRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetReviewByUsernameAsync_ValidUsername_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";

    var review = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
    };
    _context.Reviews.Add(review);

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
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewResponse = autoMapper
      .mapper.ProjectTo<ReviewResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.id == 1).Should().Be(reviewResponse);
  }

  [Fact]
  public async Task GetReviewByUsernameAsync_OrderedByNewest_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";

    var review1 = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
      CreatedAt = DateTime.UnixEpoch,
    };
    _context.Reviews.Add(review1);

    var review2 = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
      CreatedAt = DateTime.UnixEpoch.AddSeconds(1),
    };
    _context.Reviews.Add(review2);

    var review3 = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
      CreatedAt = DateTime.UnixEpoch.AddSeconds(2),
    };
    _context.Reviews.Add(review3);

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
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    // Act
    var actual = await reviewRepository.GetReviewByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(3);
    actual.Data.FirstOrDefault(r => r.id == 3).Should().Be(actual.Data.First());
  }

  [Fact]
  public async Task GetReviewByUsernameAsync_ValidUsername_WithValidRating_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";
    var ratingValue = 5;

    var review = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
    };
    _context.Reviews.Add(review);

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

    var rating = new UserRating()
    {
      BookId = book.Id,
      UserId = user.Id,
      Rating = ratingValue,
    };
    _context.UserRatings.Add(rating);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewResponse = autoMapper
      .mapper.ProjectTo<ReviewResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.id == 1).Should().Be(reviewResponse);
    actual.Data.FirstOrDefault(r => r.id == 1)?.rating.Should().Be(ratingValue);
  }

  [Fact]
  public async Task GetReviewByUsernameAsync_ValidUsername_WithNullRating_ReturnsValidResponse()
  {
    // Arrange
    var username = "test";

    var review = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
    };
    _context.Reviews.Add(review);

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
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewResponse = autoMapper
      .mapper.ProjectTo<ReviewResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByUsernameAsync(10, 1, username);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.id == 1).Should().Be(reviewResponse);
    actual.Data.FirstOrDefault(r => r.id == 1)?.rating.Should().BeNull();
  }

  [Fact]
  public async Task GetReviewByUsernameAsync_InvalidUsername_ReturnsEmptyList()
  {
    // Arrange
    var username = "test";

    var review = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
    };
    _context.Reviews.Add(review);

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
      Username = "invalid-username",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewResponse = autoMapper
      .mapper.ProjectTo<ReviewResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByUsernameAsync(10, 1, username);

    // Assert
    actual.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task GetReviewByBookAsync_ValidBookId_ReturnsValidResponse()
  {
    // Arrange
    var bookId = 1;

    var review = new Review()
    {
      UserId = 1,
      BookId = bookId,
      Text = "",
    };
    _context.Reviews.Add(review);

    var book = new Book()
    {
      Id = bookId,
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
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewBookResponse = autoMapper
      .mapper.ProjectTo<ReviewBookResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByBookAsync(10, 1, bookId);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.id == 1).Should().Be(reviewBookResponse);
  }

  [Fact]
  public async Task GetReviewByBookAsync_OrderedByNewest_ReturnsValidResponse()
  {
    // Arrange
    var bookId = 1;

    var review1 = new Review()
    {
      UserId = 1,
      BookId = bookId,
      Text = "",
      CreatedAt = DateTime.UnixEpoch,
    };
    _context.Reviews.Add(review1);

    var review2 = new Review()
    {
      UserId = 1,
      BookId = bookId,
      Text = "",
      CreatedAt = DateTime.UnixEpoch.AddSeconds(1),
    };
    _context.Reviews.Add(review2);

    var review3 = new Review()
    {
      UserId = 1,
      BookId = bookId,
      Text = "",
      CreatedAt = DateTime.UnixEpoch.AddSeconds(2),
    };
    _context.Reviews.Add(review3);

    var book = new Book()
    {
      Id = bookId,
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
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    // Act
    var actual = await reviewRepository.GetReviewByBookAsync(10, 1, bookId);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.id == 3).Should().Be(actual.Data.First());
  }

  [Fact]
  public async Task GetReviewByBookAsync_ValidBookId_WithValidRating_ReturnsValidResponse()
  {
    // Arrange
    var bookId = 1;
    var ratingValue = 5;

    var review = new Review()
    {
      UserId = 1,
      BookId = bookId,
      Text = "",
    };
    _context.Reviews.Add(review);

    var book = new Book()
    {
      Id = bookId,
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

    var rating = new UserRating()
    {
      BookId = book.Id,
      UserId = user.Id,
      Rating = ratingValue,
    };
    _context.UserRatings.Add(rating);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewBookResponse = autoMapper
      .mapper.ProjectTo<ReviewBookResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByBookAsync(10, 1, bookId);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.id == 1).Should().Be(reviewBookResponse);
    actual.Data.FirstOrDefault(r => r.id == 1)?.rating.Should().Be(ratingValue);
  }

  [Fact]
  public async Task GetReviewByBookAsync_ValidBookId_WithNullRating_ReturnsValidResponse()
  {
    // Arrange
    var bookId = 1;

    var review = new Review()
    {
      UserId = 1,
      BookId = bookId,
      Text = "",
    };
    _context.Reviews.Add(review);

    var book = new Book()
    {
      Id = bookId,
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
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewBookResponse = autoMapper
      .mapper.ProjectTo<ReviewBookResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByBookAsync(10, 1, bookId);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.FirstOrDefault(r => r.id == 1).Should().Be(reviewBookResponse);
    actual.Data.FirstOrDefault(r => r.id == 1)?.rating.Should().BeNull();
  }

  [Fact]
  public async Task GetReviewByBookAsync_InvalidBookId_ReturnsEmptyList()
  {
    // Arrange
    var bookId = 2;

    var review = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
    };
    _context.Reviews.Add(review);

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
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewResponse = autoMapper
      .mapper.ProjectTo<ReviewResponse>(QueryableUtils.MapToIQueryable(review))
      .FirstOrDefault();

    // Act
    var actual = await reviewRepository.GetReviewByBookAsync(10, 1, bookId);

    // Assert
    actual.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task AddReviewAsync_AddReview_ReturnsValidResponse()
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
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var review = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
    };

    // Act
    var result = await reviewRepository.AddReviewAsync(review);

    // Assert
    result.Should().NotBeNull();
    _context.Reviews.FirstOrDefault(r => r.BookId == 1 && r.UserId == 1).Should().NotBe(null);
  }

  [Fact]
  public async Task RemoveReviewAsync_RemoveReview_ReturnsValidResponse()
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
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    var review = new Review()
    {
      UserId = 1,
      BookId = 1,
      Text = "",
    };

    _context.Reviews.Add(review);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    var reviewDb = await _context.Reviews.FirstAsync(r => r.BookId == 1 && r.UserId == 1);

    // Act
    await reviewRepository.RemoveReviewAsync(reviewDb.Id);

    // Assert
    _context.Reviews.Should().HaveCount(0);
  }

  [Fact]
  public async Task RemoveReviewAsync_RemoveInvalidReview_ReturnsNull()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    // Act
    var response = await reviewRepository.RemoveReviewAsync(1);

    // Assert
    response.Should().BeNull();
    _context.Reviews.Should().HaveCount(0);
  }

  [Fact]
  public async Task PatchReviewAsync_ValidReview_ReturnsValidResponse()
  {
    // Arrange
    var createdAt = DateTime.UtcNow;
    var text = "New text";

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
      CreatedAt = createdAt,
    };

    _context.Users.Add(user);

    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var review = new Review()
    {
      Id = 1,
      Book = book,
      User = user,
      Text = "",
    };

    _context.Reviews.Add(review);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    // Act
    var actual = await reviewRepository.PatchReviewAsync(1, text);

    // Assert
    actual.Should().NotBeNull();
    actual!.text.Should().Be(text);
    _context.Reviews.Should().HaveCount(1);
  }

  [Fact]
  public async Task PatchReviewAsync_InvalidReview_ReturnsNullResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    // Act
    var actual = await reviewRepository.PatchReviewAsync(1, "");

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetReviewByIdAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
      CreatedAt = DateTime.UnixEpoch,
    };

    _context.Users.Add(user);

    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var review = new Review()
    {
      Id = 1,
      Book = book,
      User = user,
      Text = "",
    };

    _context.Reviews.Add(review);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    // Act
    var actual = await reviewRepository.GetReviewByIdAsync(1);

    // Assert
    actual.Should().NotBeNull();
    actual!.id.Should().Be(1);
  }

  [Fact]
  public async Task GetReviewByIdAsync_InvalidReview_ReturnsNull()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var reviewRepository = new ReviewRepository(_context, autoMapper.mapper);

    // Act
    var actual = await reviewRepository.GetReviewByIdAsync(1);

    // Assert
    actual.Should().BeNull();
  }
}
