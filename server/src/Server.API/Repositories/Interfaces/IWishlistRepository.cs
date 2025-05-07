using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Repositories.Interfaces;

public interface IWishlistRepository
{
  public Task<PaginatedListEnvelope<WishlistResponse>> GetWishlistByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  );
  public Task<Envelope<WishlistByBookResponse>?> GetWishlistByUsernameAndBookAsync(
    string username,
    int bookId
  );

  public Task<Wishlist?> AddWishlistAsync(Wishlist wishlist);

  public Task<Wishlist?> RemoveWishlistAsync(int userId, int bookId);
}
