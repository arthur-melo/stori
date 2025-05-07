using Server.API.Models;

namespace Server.API.Repositories.Interfaces;

public interface ISettingRepository
{
  public Task<PaginatedListEnvelope<string?>> GetSettingsAsync(
    int pageSize,
    int pageNumber,
    string? name
  );
}
