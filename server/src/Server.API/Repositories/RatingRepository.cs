using Microsoft.EntityFrameworkCore;
using Server.API.Models.Context;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class RatingRepository(StoriContext context) : IRatingRepository
{
  private readonly StoriContext _context = context;

  public async Task SaveBookRatingAsync(Rating rating)
  {
    var ratingEntity = await _context.Ratings.SingleOrDefaultAsync(r => r.BookId == rating.BookId);

    if (ratingEntity is null)
    {
      throw new Exception("Invalid rating entity");
    }

    ratingEntity.Star1 = rating.Star1;
    ratingEntity.Star2 = rating.Star2;
    ratingEntity.Star3 = rating.Star3;
    ratingEntity.Star4 = rating.Star4;
    ratingEntity.Star5 = rating.Star5;
    ratingEntity.StarsAverage = rating.StarsAverage;
    ratingEntity.StarsTotal = rating.StarsTotal;

    await _context.SaveChangesAsync();
  }

  public Task<Rating?> GetRatingByBookIdAsync(int bookId)
  {
    return _context.Ratings.AsNoTracking().Where(r => r.BookId == bookId).SingleOrDefaultAsync();
  }
}
