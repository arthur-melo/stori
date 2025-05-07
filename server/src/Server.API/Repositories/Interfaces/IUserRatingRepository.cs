using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Repositories.Interfaces;

public interface IUserRatingRepository
{
  public Task<PaginatedListEnvelope<UserRatingResponse>> GetUserRatingByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  );

  public Task<Envelope<UserRatingByBookResponse>?> GetUserRatingByUsernameAndBookAsync(
    string username,
    int bookId
  );
  public Task<UserRating?> AddUserRatingAsync(UserRating userRating);

  public Task<UserRating?> RemoveUserRatingAsync(int userId, int bookId);
}
