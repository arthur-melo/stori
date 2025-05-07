using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class AwardRepository(StoriContext context, IMapper mapper) : IAwardRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<string?>> GetAwardsAsync(
    int pageSize,
    int pageNumber,
    string? name
  )
  {
    var collection = _context.Awards as IQueryable<Award>;

    // Filter by name, if present.
    if (!string.IsNullOrEmpty(name))
    {
      collection = collection.Where(award => award.Name!.Contains(name));
    }

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Ordering
    collection = collection.OrderBy(a => a.Name);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var awards = await collection
      .AsNoTracking()
      .ProjectTo<string?>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<string?>(awards, pageNumber, totalPages, count);
  }
}
