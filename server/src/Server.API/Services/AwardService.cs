using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class AwardService(IAwardRepository awardRepository) : IAwardService
{
  private readonly IAwardRepository _awardRepository = awardRepository;

  public async Task<PaginatedListEnvelope<string?>> GetAwardsAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  )
  {
    var awards = await _awardRepository.GetAwardsAsync(pageSize!.Value, pageNumber!.Value, name);

    return awards;
  }
}
