using Server.API.Exceptions;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class RatingService(IRatingRepository ratingRepository) : IRatingService
{
  private readonly IRatingRepository _ratingRepository = ratingRepository;

  public async Task AddBookRatingAsync(int bookId, int rating)
  {
    var bookRating = await _ratingRepository.GetRatingByBookIdAsync(bookId);

    if (bookRating == null)
    {
      throw new NotFoundException("No rating found.");
    }

    // If the new bookrating is null, assign it to 1, otherwise add to it.
    switch (rating)
    {
      case 1:
        bookRating.Star1 = bookRating.Star1 is null ? 1 : bookRating.Star1 + 1;
        break;
      case 2:
        bookRating.Star2 = bookRating.Star2 is null ? 1 : bookRating.Star2 + 1;
        break;
      case 3:
        bookRating.Star3 = bookRating.Star3 is null ? 1 : bookRating.Star3 + 1;
        break;
      case 4:
        bookRating.Star4 = bookRating.Star4 is null ? 1 : bookRating.Star4 + 1;
        break;
      case 5:
        bookRating.Star5 = bookRating.Star5 is null ? 1 : bookRating.Star5 + 1;
        break;
      default:
        throw new Exception("Invalid rating value");
    }

    bookRating.StarsTotal = GetStarsTotalSum(bookRating);
    bookRating.StarsAverage = GetStarsWeightedAverage(bookRating);

    await _ratingRepository.SaveBookRatingAsync(bookRating);
  }

  public async Task DeleteBookRatingAsync(int bookId, int rating)
  {
    var bookRating = await _ratingRepository.GetRatingByBookIdAsync(bookId);

    if (bookRating == null)
    {
      throw new NotFoundException("No rating found.");
    }

    // If the new bookrating is 0 or null, assign it to null, otherwise subtract from it.
    switch (rating)
    {
      case 1:
        if (bookRating.Star1 is not null)
        {
          bookRating.Star1 = bookRating.Star1 - 1 == 0 ? null : bookRating.Star1 - 1;
        }
        break;
      case 2:
        if (bookRating.Star2 is not null)
        {
          bookRating.Star2 = bookRating.Star2 - 1 == 0 ? null : bookRating.Star2 - 1;
        }
        break;
      case 3:
        if (bookRating.Star3 is not null)
        {
          bookRating.Star3 = bookRating.Star3 - 1 == 0 ? null : bookRating.Star3 - 1;
        }
        break;
      case 4:
        if (bookRating.Star4 is not null)
        {
          bookRating.Star4 = bookRating.Star4 - 1 == 0 ? null : bookRating.Star4 - 1;
        }
        break;
      case 5:
        if (bookRating.Star5 is not null)
        {
          bookRating.Star5 = bookRating.Star5 - 1 == 0 ? null : bookRating.Star5 - 1;
        }
        break;
      default:
        throw new Exception("Invalid rating value");
    }

    bookRating.StarsTotal = GetStarsTotalSum(bookRating);
    bookRating.StarsAverage = GetStarsWeightedAverage(bookRating);

    await _ratingRepository.SaveBookRatingAsync(bookRating);
  }

  private int? GetStarsTotalSum(Rating rating)
  {
    // If the total sum is 0, assign it to null.
    var starsTotalSum =
      (rating.Star1 ?? 0)
      + (rating.Star2 ?? 0)
      + (rating.Star3 ?? 0)
      + (rating.Star4 ?? 0)
      + (rating.Star5 ?? 0);

    return starsTotalSum == 0 ? null : starsTotalSum;
  }

  private double? GetStarsWeightedAverage(Rating rating)
  {
    // If the weighted average is 0, assign it to null.
    var starsAverage =
      (
        (rating.Star1 ?? 0) * 1
        + (rating.Star2 ?? 0) * 2
        + (rating.Star3 ?? 0) * 3
        + (rating.Star4 ?? 0) * 4
        + (rating.Star5 ?? 0) * 5
      ) / (rating.StarsTotal ?? 1.0);

    return starsAverage == 0 ? null : Math.Round(starsAverage, 2);
  }
}
