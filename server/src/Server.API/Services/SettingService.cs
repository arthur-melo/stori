using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class SettingService(ISettingRepository settingRepository) : ISettingService
{
  private readonly ISettingRepository _settingRepository = settingRepository;

  public async Task<PaginatedListEnvelope<string?>> GetSettingsAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  )
  {
    var settings = await _settingRepository.GetSettingsAsync(
      pageSize!.Value,
      pageNumber!.Value,
      name
    );

    return settings;
  }
}
