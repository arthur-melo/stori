namespace Server.API.Services.Interfaces;

public interface IRatingService
{
  public Task AddBookRatingAsync(int bookId, int rating);

  public Task DeleteBookRatingAsync(int bookId, int rating);
}
