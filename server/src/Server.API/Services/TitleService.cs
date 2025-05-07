using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class TitleService(ITitleRepository titleRepository) : ITitleService
{
  private readonly ITitleRepository _titleRepository = titleRepository;

  public async Task<PaginatedListEnvelope<string?>> GetTitlesAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  )
  {
    var titles = await _titleRepository.GetTitlesAsync(pageSize!.Value, pageNumber!.Value, name);

    return titles;
  }
}
