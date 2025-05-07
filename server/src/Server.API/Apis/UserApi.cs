using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class UserApi
{
  public static RouteGroupBuilder MapUserApi(this RouteGroupBuilder app)
  {
    app.MapGet("/users", GetAuthorizedUserAsync).RequireAuthorization();
    app.MapGet("/users/{username}", GetUserAsync);
    app.MapPatch("/users/{username}", PatchUserAsync).RequireAuthorization();
    app.MapPost("/users/{username}/upload", PostUserPhotoAsync)
      .DisableAntiforgery()
      .RequireAuthorization();
    app.MapDelete("/users/{username}/upload", RemoveUserPhotoAsync).RequireAuthorization();

    return app;
  }

  /// <summary>
  /// Returns the current authorized user data.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/users
  ///
  /// </remarks>
  /// <response code="200">Returns the given authorized user data</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<Envelope<UserAuthorizedResponse>>, BadRequest, NotFound>
  > GetAuthorizedUserAsync([FromServices] IUserService userService, HttpContext httpContext)
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    var userResponse = await userService.GetUserByIdAsync(Convert.ToInt32(id!.Value));

    return TypedResults.Ok(userResponse);
  }

  /// <summary>
  /// Returns a given user data
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/users/{username}
  ///
  /// </remarks>
  /// <param name="request">User data</param>
  /// <response code="200">Returns the given `username` data</response>
  /// <response code="404">If the given `username` was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<
    Results<Ok<Envelope<UserUnauthorizedResponse>>, NotFound, BadRequest>
  > GetUserAsync([FromServices] IUserService userService, [AsParameters] UserRequest request)
  {
    var userResponse = await userService.GetUserByUsernameAsync(request.username);

    return TypedResults.Ok(userResponse);
  }

  /// <summary>
  /// Patches a given user data
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     PATCH /api/v1/users/{username}
  ///     {
  ///       "username": "new-username",
  ///       "name": "New name",
  ///       "email": "newEmail@example.com",
  ///       "password": "newPassword",
  ///     }
  ///
  /// All body parameters are optional
  ///
  /// </remarks>
  /// <param name="requestBody">User data</param>
  /// <response code="200">Returns the given `username` updated data</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<Envelope<UserAuthorizedResponse>>, BadRequest, NotFound, ValidationProblem>
  > PatchUserAsync(
    [FromServices] IUserService userService,
    [FromBody] UserPatchRequestBody requestBody,
    [AsParameters] UserPatchRequestParameters requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    var userResponse = await userService.PatchUserAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username!,
      requestBody.email,
      requestBody.password,
      requestBody.username,
      requestBody.name
    );

    return TypedResults.Ok(userResponse);
  }

  /// <summary>
  /// Uploads a profile image to a given user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/users/{username}/upload
  ///     {
  ///       "profileImg": FormData,
  ///     }
  ///
  /// </remarks>
  /// <param name="userPostRequestForm">User data</param>
  /// <response code="201">Returns an empty response when the image gets uploaded to the given `username`</response>
  /// <response code="400">If the parameters validation failed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Created, NotFound, BadRequest, ValidationProblem>
  > PostUserPhotoAsync(
    [FromServices] IUserService userService,
    [AsParameters] UserPostRequestForm userPostRequestForm,
    [AsParameters] UserPatchRequestParameters requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    var filename = await userService.PostUserPhotoAsync(
      Convert.ToInt32(id!.Value),
      requestParams.username!,
      userPostRequestForm.profileImg!
    );

    IHeaderDictionary headers = httpContext.Response.Headers;
    headers.Append("Location", $"/images/{filename}");

    return TypedResults.Created();
  }

  /// <summary>
  /// Removes the photo from the user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     DELETE /api/v1/users/{username}/upload
  ///
  /// </remarks>
  /// <response code="204">Returns an empty response when the user photo is removed</response>
  /// <response code="401">If the authentication failed</response>
  /// <response code="404">If the given `username` was not found</response>
  [Authorize]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<NoContent, NotFound, BadRequest, ValidationProblem>
  > RemoveUserPhotoAsync(
    [FromServices] IUserService userService,
    [AsParameters] UserPatchRequestParameters requestParams,
    HttpContext httpContext
  )
  {
    var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

    await userService.RemoveUserPhotoAsync(Convert.ToInt32(id!.Value), requestParams.username!);

    return TypedResults.NoContent();
  }
}
