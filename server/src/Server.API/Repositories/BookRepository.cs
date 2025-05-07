using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class BookRepository(StoriContext context, IMapper mapper) : IBookRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<BookListResponse>> GetBooksAsync(
    int pageSize,
    int pageNumber,
    string orderBy,
    string? genre,
    string? title,
    string? character,
    string? award,
    string? setting
  )
  {
    var collection = _context.Books as IQueryable<Book>;

    // Filter by genre, if present.
    if (!string.IsNullOrEmpty(genre))
    {
      collection = collection.Where(book => book.Genres.Any(g => g.Name!.Contains(genre)));
    }

    // Filter by title, if present.
    if (!string.IsNullOrEmpty(title))
    {
      collection = collection.Where(b => b.Title!.Contains(title));
    }

    // Filter by character, if present.
    if (!string.IsNullOrEmpty(character))
    {
      collection = collection.Where(book => book.Characters.Any(c => c.Name!.Contains(character)));
    }

    // Filter by award, if present.
    if (!string.IsNullOrEmpty(award))
    {
      collection = collection.Where(book => book.Awards.Any(a => a.Name!.Contains(award)));
    }

    // Filter by setting, if present.
    if (!string.IsNullOrEmpty(setting))
    {
      collection = collection.Where(book => book.Settings.Any(s => s.Name!.Contains(setting)));
    }

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Ordering
    if (!string.IsNullOrEmpty(orderBy))
    {
      collection = orderBy switch
      {
        "date" => collection.OrderByDescending(b => b.PublishDate),
        "rating" => collection.OrderByDescending(b => b.Rating!.StarsTotal),
        _ => collection.OrderBy(b => b.Id),
      };
    }

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.

    var books = await collection
      .AsNoTracking()
      .ProjectTo<BookListResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<BookListResponse>(books, pageNumber, totalPages, count);
  }

  public async Task<Envelope<BookResponse>?> GetBookByIdAsync(int id)
  {
    var book = await _context
      .Books.AsNoTracking()
      .Where(b => b.Id == id)
      .ProjectTo<BookResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    if (!book.Any())
    {
      return null;
    }

    return new Envelope<BookResponse>(book);
  }

  public async Task<Envelope<BookResponse>?> GetBookByBookIdAsync(string bookId)
  {
    var book = await _context
      .Books.AsNoTracking()
      .Where(b => b.BookId == bookId)
      .ProjectTo<BookResponse>(_mapper.ConfigurationProvider)
      .ToListAsync();

    if (!book.Any())
    {
      return null;
    }

    return new Envelope<BookResponse>(book);
  }

  public async Task<bool> IsBookInDatabaseAsync(int bookId)
  {
    var book = await _context.Books.FindAsync(bookId);

    return book is not null;
  }
}
