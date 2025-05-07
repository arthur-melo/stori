using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class GenreApi
{
  public static RouteGroupBuilder MapGenresApi(this RouteGroupBuilder app)
  {
    app.MapGet("/genres", GetGenresAsync);

    return app;
  }

  /// <summary>
  /// Returns a list of book genres
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/genres?pageSize=10&amp;pageNumber=1&amp;name=filterGenreByName
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Genre data</param>
  /// <response code="200">Returns a paginated list of book genres</response>
  /// <response code="400">If the parameters validation failed</response>
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedListEnvelope<string?>))]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<string?>>, ValidationProblem>
  > GetGenresAsync(
    [FromServices] IGenreService genreService,
    [AsParameters] PaginatedListRequest request
  )
  {
    var genres = await genreService.GetGenresAsync(
      request.pageSize,
      request.pageNumber,
      request.name
    );

    return TypedResults.Ok(genres);
  }
}
