using Server.API.Models;

namespace Server.API.Services.Interfaces;

public interface IAwardService
{
  public Task<PaginatedListEnvelope<string?>> GetAwardsAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  );
}
