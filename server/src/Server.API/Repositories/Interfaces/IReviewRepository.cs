using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Repositories.Interfaces;

public interface IReviewRepository
{
  public Task<PaginatedListEnvelope<ReviewResponse>> GetReviewByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  );

  public Task<PaginatedListEnvelope<ReviewBookResponse>> GetReviewByBookAsync(
    int pageSize,
    int pageNumber,
    int bookId
  );

  public Task<ReviewBookResponse?> GetReviewByIdAsync(int reviewId);

  public Task<Review?> AddReviewAsync(Review review);
  public Task<Review?> RemoveReviewAsync(int reviewId);

  public Task<ReviewBookResponse?> PatchReviewAsync(int reviewId, string text);
}
