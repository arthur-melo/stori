using Server.API.Models;

namespace Server.API.Repositories.Interfaces;

public interface ITitleRepository
{
  public Task<PaginatedListEnvelope<string?>> GetTitlesAsync(
    int pageSize,
    int pageNumber,
    string? name
  );
}
