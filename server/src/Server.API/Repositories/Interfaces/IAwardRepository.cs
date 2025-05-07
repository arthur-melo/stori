using Server.API.Models;

namespace Server.API.Repositories.Interfaces;

public interface IAwardRepository
{
  public Task<PaginatedListEnvelope<string?>> GetAwardsAsync(
    int pageSize,
    int pageNumber,
    string? name
  );
}
