using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class CharacterService(ICharacterRepository characterRepository) : ICharacterService
{
  private readonly ICharacterRepository _characterRepository = characterRepository;

  public async Task<PaginatedListEnvelope<string?>> GetCharactersAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  )
  {
    var characters = await _characterRepository.GetCharactersAsync(
      pageSize!.Value,
      pageNumber!.Value,
      name
    );

    return characters;
  }
}
