using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class ReviewRepository(StoriContext context, IMapper mapper) : IReviewRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<ReviewResponse>> GetReviewByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  )
  {
    var collection = _context.Reviews as IQueryable<Review>;

    collection = collection
      .Where(r => r.User.Username == username)
      .OrderByDescending(r => r.CreatedAt)
      .AsQueryable();

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var reviews = await collection
      .AsNoTracking()
      .ProjectTo<ReviewResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<ReviewResponse>(reviews, pageNumber, totalPages, count);
  }

  public async Task<PaginatedListEnvelope<ReviewBookResponse>> GetReviewByBookAsync(
    int pageSize,
    int pageNumber,
    int bookId
  )
  {
    var collection = _context.Reviews as IQueryable<Review>;

    collection = _context
      .Reviews.Where(r => r.BookId == bookId)
      .OrderByDescending(r => r.CreatedAt)
      .AsQueryable();

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var reviews = await collection
      .AsNoTracking()
      .ProjectTo<ReviewBookResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<ReviewBookResponse>(reviews, pageNumber, totalPages, count);
  }

  public async Task<Review?> AddReviewAsync(Review review)
  {
    var result = await _context.Reviews.AddAsync(review);
    await _context.SaveChangesAsync();

    return result.Entity;
  }

  public async Task<Review?> RemoveReviewAsync(int reviewId)
  {
    var existingReview = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);

    if (existingReview is null)
    {
      return null;
    }

    var result = _context.Reviews.Remove(existingReview);

    await _context.SaveChangesAsync();

    return result.Entity;
  }

  public async Task<ReviewBookResponse?> GetReviewByIdAsync(int reviewId)
  {
    return await _context
      .Reviews.Where(r => r.Id == reviewId)
      .ProjectTo<ReviewBookResponse>(_mapper.ConfigurationProvider)
      .FirstOrDefaultAsync();
  }

  public async Task<ReviewBookResponse?> PatchReviewAsync(int reviewId, string text)
  {
    var review = await _context.Reviews.SingleOrDefaultAsync(r => r.Id == reviewId);

    if (review is null)
    {
      return null;
    }

    review.Text = text;

    _context.SaveChanges();

    return _mapper.Map<Review, ReviewBookResponse>(review);
  }
}
