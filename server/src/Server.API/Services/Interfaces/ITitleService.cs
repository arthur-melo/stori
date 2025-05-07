using Server.API.Models;

namespace Server.API.Services.Interfaces;

public interface ITitleService
{
  public Task<PaginatedListEnvelope<string?>> GetTitlesAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  );
}
