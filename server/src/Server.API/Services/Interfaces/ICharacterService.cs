using Server.API.Models;

namespace Server.API.Services.Interfaces;

public interface ICharacterService
{
  public Task<PaginatedListEnvelope<string?>> GetCharactersAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  );
}
