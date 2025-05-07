using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class UserRatingRepository(StoriContext context, IMapper mapper) : IUserRatingRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<UserRatingResponse>> GetUserRatingByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  )
  {
    var collection = _context.UserRatings as IQueryable<UserRating>;

    collection = collection.Where(ur => ur.User.Username == username).OrderBy(ur => ur.CreatedAt);

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var userRatings = await collection
      .AsNoTracking()
      .ProjectTo<UserRatingResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<UserRatingResponse>(
      userRatings,
      pageNumber,
      totalPages,
      count
    );
  }

  public async Task<Envelope<UserRatingByBookResponse>?> GetUserRatingByUsernameAndBookAsync(
    string username,
    int bookId
  )
  {
    var userRating = await _context
      .UserRatings.Where(ur => ur.User.Username == username)
      .Where(ur => ur.BookId == bookId)
      .ProjectTo<UserRatingByBookResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    if (!userRating.Any())
    {
      return null;
    }

    return new Envelope<UserRatingByBookResponse>(userRating);
  }

  public async Task<UserRating?> AddUserRatingAsync(UserRating userRating)
  {
    var existingUserRating = await _context.UserRatings.FirstOrDefaultAsync(ur =>
      ur.UserId == userRating.UserId && ur.BookId == userRating.BookId
    );

    if (existingUserRating is not null)
    {
      return null;
    }

    var result = await _context.UserRatings.AddAsync(userRating);
    await _context.SaveChangesAsync();

    return result.Entity;
  }

  public async Task<UserRating?> RemoveUserRatingAsync(int userId, int bookId)
  {
    var existingUserRating = await _context.UserRatings.FirstOrDefaultAsync(ur =>
      ur.UserId == userId && ur.BookId == bookId
    );

    if (existingUserRating is null)
    {
      return null;
    }

    var result = _context.UserRatings.Remove(existingUserRating);

    await _context.SaveChangesAsync();

    return result.Entity;
  }
}
