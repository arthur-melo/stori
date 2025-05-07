using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Server.API.Exceptions;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class AuthApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task RevokeTokenAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var email = "example@email.com";

    var userToken = await Utils.CreateUserAndGetTokenAsync(_httpClient, "username", email);

    // Act
    var revokeTokenRequest = new RefreshTokenRequest(userToken!.refreshToken!.token);

    var revokeTokenResponse = await _httpClient.SendAsync(
      new HttpRequestMessage()
      {
        Content = new StringContent(
          JsonSerializer.Serialize(revokeTokenRequest),
          Encoding.UTF8,
          "application/json"
        ),
        Method = HttpMethod.Delete,
        RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/auth/token/revoke"),
      }
    );

    // Assert
    revokeTokenResponse.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task RevokeTokenAsync_InvalidParameters_ReturnErrorResponse()
  {
    // Arrange
    await Utils.CreateUserAndGetTokenAsync(_httpClient, "username", "example@email.com");

    // Act
    var revokeTokenRequest = new RefreshTokenRequest("invalid");

    var revokeTokenResponse = await _httpClient.SendAsync(
      new HttpRequestMessage()
      {
        Content = new StringContent(
          JsonSerializer.Serialize(revokeTokenRequest),
          Encoding.UTF8,
          "application/json"
        ),
        Method = HttpMethod.Delete,
        RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/auth/token/revoke"),
      }
    );
    var parsedTokenContent = await revokeTokenResponse.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    revokeTokenResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    parsedTokenContent.Should().NotBeNull();
    parsedTokenContent!.Type.Should().Be(nameof(NotFoundException));
  }

  [Fact]
  public async Task RefreshTokenAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      "username",
      "example@email.com"
    );

    // Act
    var refreshTokenRequest = new RefreshTokenRequest(userToken!.refreshToken!.token);
    var refreshTokenResponse = await _httpClient.PostAsJsonAsync(
      "/api/v1/auth/token/refresh",
      refreshTokenRequest
    );
    var parsedTokenContent = await refreshTokenResponse.Content.ReadFromJsonAsync<TokenResponse>();

    // Assert
    refreshTokenResponse.EnsureSuccessStatusCode();

    parsedTokenContent.Should().NotBeNull();
    parsedTokenContent!.accessToken.Should().NotBeNull();
    parsedTokenContent!.refreshToken.Should().NotBeNull();
  }

  [Fact]
  public async Task RefreshTokenAsync_InvalidParameters_ReturnErrorResponse()
  {
    // Arrange
    await Utils.CreateUserAndGetTokenAsync(_httpClient, "username", "example@email.com");

    // Act
    var refreshTokenRequest = new RefreshTokenRequest("invalid");
    var refreshTokenResponse = await _httpClient.PostAsJsonAsync(
      "/api/v1/auth/token/refresh",
      refreshTokenRequest
    );
    var parsedTokenContent = await refreshTokenResponse.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    parsedTokenContent.Should().NotBeNull();
    parsedTokenContent!.Type.Should().Be(nameof(NotFoundException));
  }

  [Fact]
  public async Task SigninAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var email = "example@email.com";
    var password = "password";

    var signupRequest = new SignupRequest("username", "name", email, password);
    var signinRequest = new SigninRequest(email, password);

    // Act
    // Add user to the database.
    var signupResponse = await _httpClient.PostAsJsonAsync("/api/v1/auth/signup", signupRequest);

    // Logs the user in.
    var signinResponse = await _httpClient.PostAsJsonAsync("/api/v1/auth/signin", signinRequest);
    var parsedContent = await signinResponse.Content.ReadFromJsonAsync<TokenResponse>();

    // Assert
    signupResponse.EnsureSuccessStatusCode();
    signinResponse.EnsureSuccessStatusCode();

    parsedContent.Should().NotBeNull();
    parsedContent!.accessToken.Should().NotBeNull();
    parsedContent!.refreshToken.Should().NotBeNull();
  }

  [Fact]
  public async Task SigninAsync_InvalidCredentials_ReturnErrorResponse()
  {
    // Arrange
    var email = "example@email.com";
    var password = "password";

    var signupRequest = new SignupRequest("username", "name", email, password);
    var signinRequest = new SigninRequest(email, "invalid");

    // Act
    // Add user to the database.
    var signupResponse = await _httpClient.PostAsJsonAsync("/api/v1/auth/signup", signupRequest);

    // Atttempts to log the user in.
    var signinResponse = await _httpClient.PostAsJsonAsync("/api/v1/auth/signin", signinRequest);
    var parsedContent = await signinResponse.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    signupResponse.EnsureSuccessStatusCode();
    signinResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    parsedContent.Should().NotBeNull();
    parsedContent!.Type.Should().Be(nameof(ValidationException));
  }

  [Fact]
  public async Task SignupAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";
    var user = new SignupRequest(username, "name", "example@email.com", "password");

    // Act
    var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/signup", user);

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299

    response
      .Headers.Where(h => h.Key.Equals("Location"))
      .SelectMany(h => h.Value)
      .FirstOrDefault()
      .Should()
      .Be($"/api/v1/users/{username}");

    _context.Users.FirstOrDefault(u => u.Email.Equals(user.email)).Should().NotBeNull();
  }

  [Fact]
  public async Task SignupAsync_EmailInUse_ReturnsErrorResponse()
  {
    // Arrange
    var email = "example@email.com";

    var User = new User()
    {
      Name = "name",
      Username = "username",
      Email = email,
      Password = "password",
      CreatedAt = DateTime.UtcNow,
    };

    _context.Users.Add(User);

    await _context.SaveChangesAsync();

    var user = new SignupRequest("username2", "name", email, "password");

    // Act
    var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/signup", user);
    var parsedContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    parsedContent.Should().NotBeNull();
    parsedContent!.Type.Should().Be(nameof(ValidationException));
  }

  [Fact]
  public async Task SignupAsync_UsernameInUse_ReturnsErrorResponse()
  {
    // Arrange
    var username = "username";

    var User = new User()
    {
      Name = "name",
      Username = username,
      Email = "example@email.com",
      Password = "password",
      CreatedAt = DateTime.UtcNow,
    };

    _context.Users.Add(User);

    await _context.SaveChangesAsync();

    var user = new
    {
      username,
      name = "name",
      email = "example2@email.com",
      password = "password",
    };

    // Act
    var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/signup", user);
    var parsedContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    parsedContent.Should().NotBeNull();
    parsedContent!.Type.Should().Be(nameof(ValidationException));
  }
}
