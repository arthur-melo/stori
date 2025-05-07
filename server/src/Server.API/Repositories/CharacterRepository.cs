using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class CharacterRepository(StoriContext context, IMapper mapper) : ICharacterRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedListEnvelope<string?>> GetCharactersAsync(
    int pageSize,
    int pageNumber,
    string? name
  )
  {
    var collection = _context.Characters as IQueryable<Character>;

    // Filter by name, if present.
    if (!string.IsNullOrEmpty(name))
    {
      collection = collection.Where(character => character.Name!.Contains(name));
    }

    // Gather pagination metadata
    var count = await collection.CountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

    // Ordering
    collection = collection.OrderBy(c => c.Name);

    // Apply pagination to collection.
    collection = collection.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

    // Execute query and map to DTO.
    var characters = await collection
      .AsNoTracking()
      .ProjectTo<string?>(_mapper.ConfigurationProvider)
      .ToListAsync();

    return new PaginatedListEnvelope<string?>(characters, pageNumber, totalPages, count);
  }
}
