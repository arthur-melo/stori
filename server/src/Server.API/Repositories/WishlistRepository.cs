using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class WishlistRepository(StoriContext context, IMapper mapper) : IWishlistRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<WishlistResponse>> GetWishlistByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  )
  {
    var collection = _context.Wishlists as IQueryable<Wishlist>;

    collection = collection.Where(w => w.User.Username == username).OrderBy(w => w.CreatedAt);

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var wishlists = await collection
      .AsNoTracking()
      .ProjectTo<WishlistResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<WishlistResponse>(wishlists, pageNumber, totalPages, count);
  }

  public async Task<Envelope<WishlistByBookResponse>?> GetWishlistByUsernameAndBookAsync(
    string username,
    int bookId
  )
  {
    var wishlist = await _context
      .Wishlists.Where(w => w.User.Username == username)
      .Where(w => w.BookId == bookId)
      .ProjectTo<WishlistByBookResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    if (!wishlist.Any())
    {
      return null;
    }

    return new Envelope<WishlistByBookResponse>(wishlist);
  }

  public async Task<Wishlist?> AddWishlistAsync(Wishlist wishlist)
  {
    var existingWishlist = await _context.Wishlists.FirstOrDefaultAsync(ur =>
      ur.UserId == wishlist.UserId && ur.BookId == wishlist.BookId
    );

    if (existingWishlist is not null)
    {
      return null;
    }

    var result = await _context.Wishlists.AddAsync(wishlist);
    await _context.SaveChangesAsync();

    return result.Entity;
  }

  public async Task<Wishlist?> RemoveWishlistAsync(int userId, int bookId)
  {
    var existingWishlist = await _context.Wishlists.FirstOrDefaultAsync(ur =>
      ur.UserId == userId && ur.BookId == bookId
    );

    if (existingWishlist is null)
    {
      return null;
    }

    var result = _context.Wishlists.Remove(existingWishlist);

    await _context.SaveChangesAsync();

    return result.Entity;
  }
}
