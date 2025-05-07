using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class GenreService(IGenreRepository genreRepository) : IGenreService
{
  private readonly IGenreRepository _genreRepository = genreRepository;

  public async Task<PaginatedListEnvelope<string?>> GetGenresAsync(
    int? pageSize,
    int? pageNumber,
    string? name
  )
  {
    var genres = await _genreRepository.GetGenresAsync(pageSize!.Value, pageNumber!.Value, name);

    return genres;
  }
}
