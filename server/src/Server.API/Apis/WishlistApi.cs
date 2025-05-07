using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class WishlistApi
{
  public static RouteGroupBuilder MapWishlistApi(this RouteGroupBuilder app)
  {
    app.MapGet("/wishlists/{username}", GetWishlistAsync);
    app.MapGet("/wishlists/{username}/{bookId}", GetWishlistByBookAsync);
    app.MapPost("/wishlists/{username}/{bookId}", AddWishlistAsync).RequireAuthorization();
    app.MapDelete("/wishlists/{username}/{bookId}", RemoveWishlistAsync).RequireAuthorization();

    return app;
  }

  /// <summary>
  /// Returns the wishlist of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/wishlists/{username}
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  /// </remarks>
  /// <param name="request">Wishlist data</param>
  /// <response code="200">Returns a paginated wishlist added by a given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given username was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<WishlistResponse>>, BadRequest, NotFound, ValidationProblem>
  > GetWishlistAsync(
    [FromServices] IWishlistService wishlistService,
    [AsParameters] PaginatedUserRequest request
  )
  {
    var wishlistResponse = await wishlistService.GetWishlistAsync(
      request.pageSize!.Value,
      request.pageNumber!.Value,
      request.username
    );

    return TypedResults.Ok(wishlistResponse);
  }

  /// <summary>
  /// Returns a single wishlist book from a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/wishlist/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="request">Wishlist data</param>
  /// <response code="200">Returns a single wishlist book by a given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found, or if there is wishlist available.</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<Envelope<WishlistByBookResponse>>, NotFound, BadRequest, ValidationProblem>
  > GetWishlistByBookAsync(
    [FromServices] IWishlistService wishlistService,
    [AsParameters] WishlistRequestParams request
  )
  {
    var wishlistResponse = await wishlistService.GetWishlistByBookAsync(
      request.username!,
      request.bookId!.Value
    );

    return TypedResults.Ok(wishlistResponse);
  }

  /// <summary>
  /// Adds a book to the wishlist of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/wishlists/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="requestParams">Wishlist data</param>
  /// <response code="201">Returns an empty response when the given `bookId` was added to the `username` wishlist</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Created, BadRequest, NotFound, ValidationProblem>
  > AddWishlistAsync(
    [FromServices] IWishlistService userRatingService,
    [AsParameters] WishlistRequestParams requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    await userRatingService.AddWishlistAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username!,
      requestParams.bookId!.Value
    );

    IHeaderDictionary headers = httpContext.Response.Headers;
    headers.Append("Location", $"/api/v1/wishlists/{requestParams.username}");

    return TypedResults.Created();
  }

  /// <summary>
  /// Removes a book from the wishlist of a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     DELETE /api/v1/wishlists/{username}/{bookId}
  ///
  /// </remarks>
  /// <param name="requestParams">Wishlist data</param>
  /// <response code="204">Returns an empty response when the given `bookId` was deleted from the `username` wishlist</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` or `bookId` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<NoContent, NotFound, BadRequest, ValidationProblem>
  > RemoveWishlistAsync(
    [FromServices] IWishlistService userRatingService,
    [AsParameters] WishlistRequestParams requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    await userRatingService.RemoveWishlistAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username!,
      requestParams.bookId!.Value
    );

    return TypedResults.NoContent();
  }
}
