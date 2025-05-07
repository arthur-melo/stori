using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class AuthApi
{
  public static RouteGroupBuilder MapAuthApi(this RouteGroupBuilder app)
  {
    app.MapPost("/auth/signup", SignupAsync);
    app.MapPost("/auth/signin", SigninAsync);
    app.MapPost("/auth/token/refresh", RefreshTokenAsync);
    app.MapDelete("/auth/token/revoke", RevokeTokenAsync);

    return app;
  }

  /// <summary>
  /// Signs up a user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/auth/signup
  ///     {
  ///       "username": "some-username",
  ///       "name": "John doe",
  ///       "email": "user@example.com",
  ///       "password": "P@ssw0rd",
  ///     }
  ///
  /// </remarks>
  /// <param name="request">User data</param>
  /// <response code="201">Returns an empty response when the user is sucessfully created</response>
  /// <response code="400">If the parameters validation failed</response>
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<Results<Created, ValidationProblem>> SignupAsync(
    [FromServices] IAuthService authService,
    [FromBody] SignupRequest request,
    HttpContext context
  )
  {
    var username = await authService.SignupAsync(
      request.username!,
      request.name!,
      request.email!,
      request.password!
    );

    IHeaderDictionary headers = context.Response.Headers;
    headers.Append("Location", $"/api/v1/users/{username}");

    return TypedResults.Created();
  }

  /// <summary>
  /// Signs in a user
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/auth/signin
  ///     {
  ///       "email": "user@example.com",
  ///       "password": "P@ssw0rd",
  ///     }
  ///
  /// </remarks>
  /// <param name="request">User data</param>
  /// <response code="200">Returns a JWT access and refresh tokens for authentication</response>
  /// <response code="404">If the given `email` was not found</response>
  /// <response code="400">If the parameters validation/authentication failed</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<Results<Ok<TokenResponse>, NotFound, ValidationProblem>> SigninAsync(
    [FromServices] IAuthService authService,
    [FromBody] SigninRequest request
  )
  {
    var response = await authService.SigninAsync(request.email!, request.password!);

    return TypedResults.Ok(response);
  }

  /// <summary>
  /// Generates a new access token from a given refresh token. A new token pair will be generated.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     POST /api/v1/auth/token/refresh
  ///     {
  ///       "token": "some-refresh-token",
  ///     }
  ///
  /// </remarks>
  /// <param name="request">User data</param>
  /// <response code="200">Returns a JWT access and refresh tokens for authentication</response>
  /// <response code="404">If the given `token` was not found</response>
  /// <response code="400">If the parameters/credentials validation failed, or if the token is expired</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<TokenResponse>, NotFound, ValidationProblem>
  > RefreshTokenAsync(
    [FromServices] IAuthService authService,
    [FromBody] RefreshTokenRequest request
  )
  {
    var response = await authService.RefreshTokenAsync(request.token!);

    return TypedResults.Ok(response);
  }

  /// <summary>
  /// Revokes a refresh token
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     DELETE /api/v1/auth/token/revoke
  ///     {
  ///       "token": "some-refresh-token",
  ///       "email": "user@example.com",
  ///     }
  ///
  /// </remarks>
  /// <param name="request">User data</param>
  /// <response code="204">Returns an empty response when the token is revoked</response>
  /// <response code="404">If the given `token` was not found</response>
  /// <response code="400">If the parameters/credentials validation failed</response>
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<Results<NoContent, NotFound, ValidationProblem>> RevokeTokenAsync(
    [FromServices] IAuthService authService,
    [FromBody] RefreshTokenRequest request
  )
  {
    await authService.RevokeRefreshTokenAsync(request.token!);

    return TypedResults.NoContent();
  }
}
