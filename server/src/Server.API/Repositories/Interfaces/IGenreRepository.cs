using Server.API.Models;

namespace Server.API.Repositories.Interfaces;

public interface IGenreRepository
{
  public Task<PaginatedListEnvelope<string?>> GetGenresAsync(
    int pageSize,
    int pageNumber,
    string? name
  );
}
