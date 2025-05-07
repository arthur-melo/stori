using Server.API.Models.Entities;

namespace Server.API.Repositories.Interfaces;

public interface IRatingRepository
{
  public Task SaveBookRatingAsync(Rating rating);

  public Task<Rating?> GetRatingByBookIdAsync(int bookId);
}
