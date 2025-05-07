using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class UserRatingApi
{
  public static RouteGroupBuilder MapUserRatingApi(this RouteGroupBuilder app)
  {
    app.MapGet("/user_ratings/{username}", GetUserRatingAsync);
    app.MapGet("/user_ratings/{username}/{bookId}", GetUserRatingByBookAsync);
    app.MapPost("/user_ratings/{username}/{bookId}", AddUserRatingAsync).RequireAuthorization();
    app.MapDelete("/user_ratings/{username}/{bookId}", RemoveUserRatingAsync)
      .RequireAuthorization();

    return app;
  }

  /// <summary>
  /// Returns the book ratings of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/user_ratings/{username}
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">User Rating data</param>
  /// <response code="200">Returns a paginated user rating list in chronological order by a given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given `username` was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<UserRatingResponse>>, NotFound, BadRequest, ValidationProblem>
  > GetUserRatingAsync(
    [FromServices] IUserRatingService userRatingService,
    [AsParameters] PaginatedUserRequest request
  )
  {
    var userResponse = await userRatingService.GetUserRatingAsync(
      request.pageSize!.Value,
      request.pageNumber!.Value,
      request.username
    );

    return TypedResults.Ok(userResponse);
  }

  /// <summary>
  /// Returns a given book rating from a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/user_ratings/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="request">User Rating data</param>
  /// <response code="200">Returns a single book rating by a given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found, or if there is no book rating available.</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<Envelope<UserRatingByBookResponse>>, NotFound, BadRequest, ValidationProblem>
  > GetUserRatingByBookAsync(
    [FromServices] IUserRatingService userRatingService,
    [AsParameters] UserRatingRequestParams request
  )
  {
    var userResponse = await userRatingService.GetUserRatingByBookAsync(
      request.username!,
      request.bookId!.Value
    );

    return TypedResults.Ok(userResponse);
  }

  /// <summary>
  /// Adds a new user rating to a given book
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/user_ratings/{username}/{bookId}
  ///     {
  ///       "rating": 5
  ///     }
  ///
  /// </remarks>
  /// <param name="requestBody">User Rating data</param>
  /// <response code="201">Returns an empty response when a given rating is added to a book</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Created, BadRequest, ValidationProblem, NotFound>
  > AddUserRatingAsync(
    [FromServices] IUserRatingService userRatingService,
    [AsParameters] UserRatingRequestParams requestParams,
    [FromBody] UserRatingRequestBody requestBody,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    var user = await userRatingService.AddUserRatingAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username!,
      requestParams.bookId!.Value,
      requestBody.rating!.Value
    );

    IHeaderDictionary headers = httpContext.Response.Headers;
    headers.Append("Location", $"/api/v1/user_ratings/{user}");

    return TypedResults.Created();
  }

  /// <summary>
  /// Removes a user rating from the user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     DELETE /api/v1/user_ratings/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="requestParams">User Rating data</param>
  /// <response code="204">Returns an empty response when a given rating is removed from a book</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<NoContent, NotFound, BadRequest, ValidationProblem>
  > RemoveUserRatingAsync(
    [FromServices] IUserRatingService userRatingService,
    [AsParameters] UserRatingRequestParams requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    await userRatingService.RemoveUserRatingAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username!,
      requestParams.bookId!.Value
    );

    return TypedResults.NoContent();
  }
}
