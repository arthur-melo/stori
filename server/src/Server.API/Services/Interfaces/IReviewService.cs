using Server.API.Models;
using Server.API.Models.Dtos.Responses;

namespace Server.API.Services.Interfaces;

public interface IReviewService
{
  public Task<PaginatedListEnvelope<ReviewResponse>> GetReviewByUsernameAsync(
    int? pageSize,
    int? pageNumber,
    string username
  );

  public Task<PaginatedListEnvelope<ReviewBookResponse>> GetReviewByBookAsync(
    int? pageSize,
    int? pageNumber,
    int bookId
  );

  public Task<string> AddReviewByBookAsync(int userId, int bookId, string text);

  public Task<string> PatchReviewByIdAsync(int userId, int reviewId, string text);

  public Task RemoveReviewAsync(int userId, string username, int reviewId);
}
