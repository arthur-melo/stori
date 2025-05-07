using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class CharacterApi
{
  public static RouteGroupBuilder MapCharactersApi(this RouteGroupBuilder app)
  {
    app.MapGet("/characters", GetCharactersAsync);

    return app;
  }

  /// <summary>
  /// Returns a list of book characters
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/characters?pageSize=10&amp;pageNumber=1&amp;name=filterCharacterByName
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Character data</param>
  /// <response code="200">Returns a paginated list of book characters</response>
  /// <response code="400">If the parameters validation failed</response>
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedListEnvelope<string?>))]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<string?>>, ValidationProblem>
  > GetCharactersAsync(
    [FromServices] ICharacterService characterService,
    [AsParameters] PaginatedListRequest request
  )
  {
    var characters = await characterService.GetCharactersAsync(
      request.pageSize,
      request.pageNumber,
      request.name
    );

    return TypedResults.Ok(characters);
  }
}
