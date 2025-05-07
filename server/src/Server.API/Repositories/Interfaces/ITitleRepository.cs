using Server.API.Models;

namespace Server.API.Repositories.Interfaces;

public interface ICharacterRepository
{
  public Task<PaginatedListEnvelope<string?>> GetCharactersAsync(
    int pageSize,
    int pageNumber,
    string? name
  );
}
