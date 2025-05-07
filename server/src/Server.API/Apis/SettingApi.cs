using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class SettingApi
{
  public static RouteGroupBuilder MapSettingsApi(this RouteGroupBuilder app)
  {
    app.MapGet("/settings", GetSettingsAsync);

    return app;
  }

  /// <summary>
  /// Returns a list of book settings
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/settings?pageSize=10&amp;pageNumber=1&amp;name=filterSettingByName
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Setting data</param>
  /// <response code="200">Returns a paginated list of book settings</response>
  /// <response code="400">If the parameters validation failed</response>
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedListEnvelope<string?>))]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<string?>>, ValidationProblem>
  > GetSettingsAsync(
    [FromServices] ISettingService genreService,
    [AsParameters] PaginatedListRequest request
  )
  {
    var settings = await genreService.GetSettingsAsync(
      request.pageSize,
      request.pageNumber,
      request.name
    );

    return TypedResults.Ok(settings);
  }
}
