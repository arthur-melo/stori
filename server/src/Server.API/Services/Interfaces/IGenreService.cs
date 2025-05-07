using Server.API.Models;

namespace Server.API.Services.Interfaces;

public interface IGenreService
{
  public Task<PaginatedListEnvelope<string?>> GetGenresAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  );
}
