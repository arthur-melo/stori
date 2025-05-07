using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class ReadlistRepository(StoriContext context, IMapper mapper) : IReadlistRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<ReadlistResponse>> GetReadlistByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  )
  {
    var collection = _context.Readlists as IQueryable<Readlist>;

    collection = collection.Where(r => r.User.Username == username).OrderBy(r => r.CreatedAt);

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var readlists = await collection
      .AsNoTracking()
      .ProjectTo<ReadlistResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<ReadlistResponse>(readlists, pageNumber, totalPages, count);
  }

  public async Task<Envelope<ReadlistByBookResponse>?> GetReadlistByUsernameAndBookAsync(
    string username,
    int bookId
  )
  {
    var readlist = await _context
      .Readlists.Where(w => w.User.Username == username)
      .Where(w => w.BookId == bookId)
      .ProjectTo<ReadlistByBookResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    if (!readlist.Any())
    {
      return null;
    }

    return new Envelope<ReadlistByBookResponse>(readlist);
  }

  public async Task<Readlist?> AddReadlistAsync(Readlist readlist)
  {
    var existingReadlist = await _context.Readlists.FirstOrDefaultAsync(ur =>
      ur.UserId == readlist.UserId && ur.BookId == readlist.BookId
    );

    if (existingReadlist is not null)
    {
      return null;
    }

    var result = await _context.Readlists.AddAsync(readlist);
    await _context.SaveChangesAsync();

    return result.Entity;
  }

  public async Task<Readlist?> RemoveReadlistAsync(int userId, int bookId)
  {
    var existingReadlist = await _context.Readlists.FirstOrDefaultAsync(ur =>
      ur.UserId == userId && ur.BookId == bookId
    );

    if (existingReadlist is null)
    {
      return null;
    }

    var result = _context.Readlists.Remove(existingReadlist);

    await _context.SaveChangesAsync();

    return result.Entity;
  }
}
