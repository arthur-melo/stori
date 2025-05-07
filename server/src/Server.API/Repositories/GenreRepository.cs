using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class GenreRepository(StoriContext context, IMapper mapper) : IGenreRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<string?>> GetGenresAsync(
    int pageSize,
    int pageNumber,
    string? name
  )
  {
    var collection = _context.Genres as IQueryable<Genre>;

    // Filter by name, if present.
    if (!string.IsNullOrEmpty(name))
    {
      collection = collection.Where(genre => genre.Name!.Contains(name));
    }

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Ordering
    collection = collection.OrderBy(g => g.Name);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var genres = await collection
      .AsNoTracking()
      .ProjectTo<string?>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<string?>(genres, pageNumber, totalPages, count);
  }
}
