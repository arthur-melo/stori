using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class ReviewApi
{
  public static RouteGroupBuilder MapReviewApi(this RouteGroupBuilder app)
  {
    app.MapGet("/reviews/{username}", GetReviewByUsernameAsync);
    app.MapDelete("/reviews/{username}", RemoveReviewByBookAsync).RequireAuthorization();
    app.MapGet("/reviews/book/{bookId}", GetReviewByBookAsync);
    app.MapPost("/reviews/book/{bookId}", AddReviewByBookAsync).RequireAuthorization();
    app.MapPatch("/reviews/patch/{reviewId}", PatchReviewByIdAsync).RequireAuthorization();

    return app;
  }

  /// <summary>
  /// Returns the book reviews of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/reviews/{username}
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Review data</param>
  /// <response code="200">Returns a paginated review list in chronological order by a given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given `username` was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<ReviewResponse>>, BadRequest, NotFound, ValidationProblem>
  > GetReviewByUsernameAsync(
    [FromServices] IReviewService reviewService,
    [AsParameters] PaginatedUserRequest request
  )
  {
    var response = await reviewService.GetReviewByUsernameAsync(
      request.pageSize!.Value,
      request.pageNumber!.Value,
      request.username
    );

    return TypedResults.Ok(response);
  }

  /// <summary>
  /// Returns the reviews of a given book
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/reviews/book/{bookId}
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Review data</param>
  /// <response code="200">Returns a paginated review list in chronological order from a given `bookId`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given `bookId` was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<ReviewBookResponse>>, BadRequest, NotFound, ValidationProblem>
  > GetReviewByBookAsync(
    [FromServices] IReviewService reviewService,
    [AsParameters] PaginatedBookRequest request
  )
  {
    var response = await reviewService.GetReviewByBookAsync(
      request.pageSize!.Value,
      request.pageNumber!.Value,
      request.bookId!.Value
    );

    return TypedResults.Ok(response);
  }

  /// <summary>
  /// Adds a new review to a given book
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/reviews/book/{bookId}
  ///     {
  ///       "text": "This is a sample book comment"
  ///     }
  ///
  /// </remarks>
  /// <param name="requestBody">Review data</param>
  /// <response code="201">Returns an empty response when a given review is added to a book</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `bookId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Created, BadRequest, NotFound, ValidationProblem>
  > AddReviewByBookAsync(
    [FromServices] IReviewService reviewService,
    [AsParameters] ReviewRequestBookParams requestParams,
    [FromBody] ReviewRequestNewCommentBody requestBody,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    var username = await reviewService.AddReviewByBookAsync(
      Convert.ToInt32(id!.Value),
      requestParams.bookId!.Value,
      requestBody.text!
    );

    IHeaderDictionary headers = httpContext.Response.Headers;
    headers.Append("Location", $"/api/v1/reviews/{username}");

    return TypedResults.Created();
  }

  /// <summary>
  /// Removes a review from the user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     DELETE /api/v1/reviews/{username}
  ///     {
  ///       "reviewId": 1
  ///     }
  ///
  /// </remarks>
  /// <param name="requestBody">Review data</param>
  /// <response code="204">Returns an empty response when a given review is removed from a book</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` or `reviewId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<NoContent, NotFound, BadRequest, ValidationProblem>
  > RemoveReviewByBookAsync(
    [FromServices] IReviewService reviewService,
    [AsParameters] ReviewRequestUsernameParams requestParams,
    [FromBody] ReviewRequestCommentBody requestBody,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    await reviewService.RemoveReviewAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username!,
      requestBody.reviewId!.Value
    );

    return TypedResults.NoContent();
  }

  /// <summary>
  /// Patches a given review
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     PATCH /api/v1/reviews/patch/{reviewId}
  ///     {
  ///       "text": "This is the new book comment"
  ///     }
  ///
  /// </remarks>
  /// <param name="requestBody">Review data</param>
  /// <response code="201">Returns an empty response when a given review is patched</response>
  /// <response code="400">If the parameters validation failed, or if the user does not have permission to edit a review.</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `reviewId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Created, BadRequest, NotFound, ValidationProblem>
  > PatchReviewByIdAsync(
    [FromServices] IReviewService reviewService,
    [AsParameters] ReviewRequestEditParams requestParams,
    [FromBody] ReviewRequestNewCommentBody requestBody,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    var username = await reviewService.PatchReviewByIdAsync(
      Convert.ToInt32(id!.Value),
      requestParams.reviewId!.Value,
      requestBody.text!
    );

    IHeaderDictionary headers = httpContext.Response.Headers;
    headers.Append("Location", $"/api/v1/reviews/{username}");

    return TypedResults.Created();
  }
}
