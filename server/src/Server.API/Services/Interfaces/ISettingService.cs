using Server.API.Models;

namespace Server.API.Services.Interfaces;

public interface ISettingService
{
  public Task<PaginatedListEnvelope<string?>> GetSettingsAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  );
}
