using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class AwardApi
{
  public static RouteGroupBuilder MapAwardsApi(this RouteGroupBuilder app)
  {
    app.MapGet("/awards", GetAwardsAsync);

    return app;
  }

  /// <summary>
  /// Returns a list of book awards
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/awards?pageSize=10&amp;pageNumber=1&amp;name=filterAwardByName
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Award data</param>
  /// <response code="200">Returns a paginated list of book awards</response>
  /// <response code="400">If the parameters validation failed</response>
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedListEnvelope<string?>))]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<string?>>, ValidationProblem>
  > GetAwardsAsync(
    [FromServices] IAwardService awardService,
    [AsParameters] PaginatedListRequest request
  )
  {
    var awards = await awardService.GetAwardsAsync(
      request.pageSize,
      request.pageNumber,
      request.name
    );

    return TypedResults.Ok(awards);
  }
}
