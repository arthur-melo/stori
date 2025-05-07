using Server.API.Models;
using Server.API.Models.Dtos.Responses;

namespace Server.API.Services.Interfaces;

public interface IWishlistService
{
  public Task<PaginatedListEnvelope<WishlistResponse>> GetWishlistAsync(
    int? pageSize,
    int? pageNumber,
    string username
  );

  public Task<Envelope<WishlistByBookResponse>> GetWishlistByBookAsync(string username, int bookId);

  public Task AddWishlistAsync(int userId, string username, int bookId);

  public Task RemoveWishlistAsync(int userId, string username, int bookId);
}
