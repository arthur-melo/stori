using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class ReadlistApi
{
  public static RouteGroupBuilder MapReadlistApi(this RouteGroupBuilder app)
  {
    app.MapGet("/readlists/{username}", GetReadlistAsync);
    app.MapGet("/readlists/{username}/{bookId}", GetReadlistByBookAsync);
    app.MapPost("/readlists/{username}/{bookId}", AddReadlistAsync).RequireAuthorization();
    app.MapDelete("/readlists/{username}/{bookId}", RemoveReadlistAsync).RequireAuthorization();

    return app;
  }

  /// <summary>
  /// Returns the readlist of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/readlists/{username}
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Readlist data</param>
  /// <response code="200">Returns a paginated readlist added by a given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given username was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<ReadlistResponse>>, BadRequest, NotFound, ValidationProblem>
  > GetReadlistAsync(
    [FromServices] IReadlistService readlistService,
    [AsParameters] PaginatedUserRequest request
  )
  {
    var readlistResponse = await readlistService.GetReadlistAsync(
      request.pageSize!.Value,
      request.pageNumber!.Value,
      request.username
    );

    return TypedResults.Ok(readlistResponse);
  }

  /// <summary>
  /// Returns a single readlist book from a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/readlist/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="request">Readlist data</param>
  /// <response code="200">Returns a single readlist book by a given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found, or if there is readlist available.</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<Envelope<ReadlistByBookResponse>>, NotFound, BadRequest, ValidationProblem>
  > GetReadlistByBookAsync(
    [FromServices] IReadlistService readlistService,
    [AsParameters] ReadlistRequestParams request
  )
  {
    var readlistResponse = await readlistService.GetReadlistByBookAsync(
      request.username!,
      request.bookId
    );

    return TypedResults.Ok(readlistResponse);
  }

  /// <summary>
  /// Adds a book to the readlist of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/readlists/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="requestParams">Readlist data</param>
  /// <response code="201">Returns an empty response when the given `bookId` was added to the `username` readlist</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Created, BadRequest, NotFound, ValidationProblem>
  > AddReadlistAsync(
    [FromServices] IReadlistService userRatingService,
    [AsParameters] ReadlistRequestParams requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    await userRatingService.AddReadlistAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username,
      requestParams.bookId
    );

    IHeaderDictionary headers = httpContext.Response.Headers;
    headers.Append("Location", $"/api/v1/readlists/{requestParams.username}");

    return TypedResults.Created();
  }

  /// <summary>
  /// Removes a book from the readlist of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     DELETE /api/v1/readlists/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="requestParams">Readlist data</param>
  /// <response code="204">Returns an empty response when the given `bookId` was deleted from the `username` readlist</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<NoContent, NotFound, BadRequest, ValidationProblem>
  > RemoveReadlistAsync(
    [FromServices] IReadlistService userRatingService,
    [AsParameters] ReadlistRequestParams requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    await userRatingService.RemoveReadlistAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username,
      requestParams.bookId
    );

    return TypedResults.NoContent();
  }
}
