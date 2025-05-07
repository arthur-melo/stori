using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class TitleApi
{
  public static RouteGroupBuilder MapTitlesApi(this RouteGroupBuilder app)
  {
    app.MapGet("/titles", GetTitlesAsync);

    return app;
  }

  /// <summary>
  /// Returns a list of book titles
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/titles?pageSize=10&amp;pageNumber=1&amp;name=filterTitleByName
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Title data</param>
  /// <response code="200">Returns a paginated list of book titles</response>
  /// <response code="400">If the parameters validation failed</response>
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedListEnvelope<string?>))]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<string?>>, ValidationProblem>
  > GetTitlesAsync(
    [FromServices] ITitleService characterService,
    [AsParameters] PaginatedListRequest request
  )
  {
    var titles = await characterService.GetTitlesAsync(
      request.pageSize,
      request.pageNumber,
      request.name
    );

    return TypedResults.Ok(titles);
  }
}
