using Server.API.Models;
using Server.API.Models.Dtos.Responses;

namespace Server.API.Services.Interfaces;

public interface IUserRatingService
{
  public Task<PaginatedListEnvelope<UserRatingResponse>> GetUserRatingAsync(
    int? pageSize,
    int? pageNumber,
    string username
  );

  public Task<Envelope<UserRatingByBookResponse>?> GetUserRatingByBookAsync(
    string username,
    int bookId
  );

  public Task<string> AddUserRatingAsync(int userId, string username, int bookId, int rating);

  public Task RemoveUserRatingAsync(int userId, string username, int bookId);
}
