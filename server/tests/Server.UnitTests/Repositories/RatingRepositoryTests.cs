using FluentAssertions;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class RatingRepositoryTests : BaseTests
{
  [Fact]
  public async Task SaveBookRatingAsync_AddRating_ReturnsValidResponse()
  {
    // Arrange
    var id = 1;

    var book = new Book()
    {
      Id = id,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var rating = new Rating() { BookId = id };

    rating.Book = book;

    _context.Ratings.Add(rating);

    await _context.SaveChangesAsync();

    var ratingRepository = new RatingRepository(_context);
    var newRating = new Rating()
    {
      BookId = id,
      Star5 = 1,
      StarsTotal = 1,
    };

    // Act
    await ratingRepository.SaveBookRatingAsync(newRating);

    // Assert
    _context.Ratings.FirstOrDefault(r => r.BookId == newRating.BookId).Should().NotBe(null);
    _context.Ratings.FirstOrDefault(r => r.BookId == newRating.BookId)!.StarsTotal.Should().Be(1);
  }

  [Fact]
  public async Task SaveBookRatingAsync_InvalidEntity_ThrowsError()
  {
    // Arrange
    var ratingRepository = new RatingRepository(_context);
    var newRating = new Rating()
    {
      BookId = 1,
      Star5 = 1,
      StarsTotal = 1,
    };

    // Act
    var act = async () => await ratingRepository.SaveBookRatingAsync(newRating);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public async Task GetRatingByBookIdAsync_GetRating_ReturnsValidResponse()
  {
    // Arrange
    var id = 1;

    var book = new Book()
    {
      Id = id,
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var rating = new Rating() { BookId = id };

    rating.Book = book;

    _context.Ratings.Add(rating);

    await _context.SaveChangesAsync();

    var ratingRepository = new RatingRepository(_context);

    // Act
    var response = await ratingRepository.GetRatingByBookIdAsync(id);

    // Assert
    response.Should().NotBeNull();
    response!.BookId.Should().Be(id);
  }
}
