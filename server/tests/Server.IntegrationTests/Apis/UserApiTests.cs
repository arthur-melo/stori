using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;
using SixLabors.ImageSharp;

namespace Server.IntegrationTests.Apis;

public class UserApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetUserAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";

    var user = new User
    {
      Name = "",
      Email = "",
      Password = "",
      Username = username,
      CreatedAt = DateTime.UtcNow,
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/users/{username}");

    var parsedContent = await response.Content.ReadFromJsonAsync<UserUnauthorizedResponse>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    _context.Users.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetAuthorizedUserAsync_ValidParameters_WithAuthorizedUser_ReturnValidResponse()
  {
    // Arrange
    var username = "username";
    var email = "example@email.com";

    var userToken = await Utils.CreateUserAndGetTokenAsync(_httpClient, username, email);

    var httpUserGetRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/users"),
    };

    httpUserGetRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    // Act
    var response = await _httpClient.SendAsync(httpUserGetRequest);

    var parsedContent = await response.Content.ReadFromJsonAsync<
      Envelope<UserAuthorizedResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.First().email.Should().Be(email);
    _context.Users.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetUserAsync_InvalidUsername_ReturnNotFoundResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/users/username");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task PatchUserAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var newEmail = "example2@email.com";
    var newPassword = "password2";
    var newUsername = "username2";
    var newName = "name2";

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      "username",
      "example@email.com"
    );

    var httpUserPatchRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Patch,
      Content = new StringContent(
        JsonSerializer.Serialize(
          new UserPatchRequestBody(newEmail, newPassword, newUsername, newName)
        ),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/users/username"),
    };

    httpUserPatchRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    var signInRequest = new SigninRequest(newEmail, newPassword);

    // Act
    var response = await _httpClient.SendAsync(httpUserPatchRequest);
    var parsedContent = await response.Content.ReadFromJsonAsync<
      Envelope<UserAuthorizedResponse>
    >();

    var loginResponse = await _httpClient.PostAsJsonAsync("/api/v1/auth/signin", signInRequest);

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.First().email.Should().Be(newEmail);
    parsedContent!.Data.First().username.Should().Be(newUsername);
    parsedContent!.Data.First().name.Should().Be(newName);
    loginResponse.EnsureSuccessStatusCode();

    _context.Users.Should().HaveCount(1);
  }

  [Fact]
  public async Task PostUserPhotoAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var image = await Image.LoadAsync(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );

    using var memoryStream = new MemoryStream();

    image.Save(memoryStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
    byte[] imageBytes = memoryStream.ToArray();

    var formContent = new MultipartFormDataContent
    {
      { new StreamContent(memoryStream), "profileImg", Constants.TEST_IMAGE_NAME },
    };

    var httpPostUserPhotoRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = formContent,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/users/{username}/upload"),
    };

    httpPostUserPhotoRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    // Act
    var response = await _httpClient.SendAsync(httpPostUserPhotoRequest);

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299

    var filename = response
      .Headers.Where(h => h.Key.Equals("Location"))
      .SelectMany(h => h.Value)
      .FirstOrDefault();

    filename.Should().NotBeNull();

    _context.Users.FirstOrDefault()!.ProfileImg.Should().NotBeNull();

    // Cleanup
    Directory.Delete(Constants.API_FILE_UPLOAD_OPTIONS_DIR, true);
  }

  [Fact]
  public async Task PostUserPhotoAsync_InvalidParameters_ReturnInvalidResponse()
  {
    // Arrange
    var username = "username";

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var image = await Image.LoadAsync(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );

    using var memoryStream = new MemoryStream();

    image.Save(memoryStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
    byte[] imageBytes = memoryStream.ToArray();

    var formContent = new MultipartFormDataContent
    {
      { new StreamContent(memoryStream), "invalid", Constants.TEST_IMAGE_NAME },
    };
    var httpPostUserPhotoRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = formContent,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/users/{username}/upload"),
    };

    httpPostUserPhotoRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    // Act
    var response = await _httpClient.SendAsync(httpPostUserPhotoRequest);
    var parsedContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    parsedContent.Should().NotBeNull();
  }

  [Fact]
  public async Task RemoveUserPhotoAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var image = await Image.LoadAsync(
      Path.Combine(Constants.TEST_IMAGE_INPUT_DIR, Constants.TEST_IMAGE_NAME)
    );

    using var memoryStream = new MemoryStream();

    image.Save(memoryStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
    byte[] imageBytes = memoryStream.ToArray();

    var formContent = new MultipartFormDataContent
    {
      { new StreamContent(memoryStream), "profileImg", Constants.TEST_IMAGE_NAME },
    };

    var httpPostUserPhotoRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = formContent,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/users/{username}/upload"),
    };

    httpPostUserPhotoRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    var imageCreationResponse = await _httpClient.SendAsync(httpPostUserPhotoRequest);

    imageCreationResponse.EnsureSuccessStatusCode(); // Status Code 200-299

    // After image creation, request the user data and verify the existence of the profile image string.
    var httpGetUserDataWithImageRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/users"),
    };

    httpGetUserDataWithImageRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    var getUserDataWithImageResponse = await _httpClient.SendAsync(httpGetUserDataWithImageRequest);

    getUserDataWithImageResponse.EnsureSuccessStatusCode(); // Status Code 200-299

    var parsedUserDataWithImageContent =
      await getUserDataWithImageResponse.Content.ReadFromJsonAsync<
        Envelope<UserAuthorizedResponse>
      >();

    parsedUserDataWithImageContent!.Data.FirstOrDefault()!.profileImg.Should().NotBeNull();

    var httpDeleteUserPhotoRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/users/{username}/upload"),
    };

    httpDeleteUserPhotoRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    // Act
    var imageDeletionResponse = await _httpClient.SendAsync(httpDeleteUserPhotoRequest);

    var httpGetUserDataWithoutImageRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/users"),
    };

    httpGetUserDataWithoutImageRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );
    var getUserDataWithoutImageResponse = await _httpClient.SendAsync(
      httpGetUserDataWithoutImageRequest
    );

    getUserDataWithoutImageResponse.EnsureSuccessStatusCode(); // Status Code 200-299

    var parsedUserDataWithoutImageContent =
      await getUserDataWithoutImageResponse.Content.ReadFromJsonAsync<
        Envelope<UserAuthorizedResponse>
      >();

    // Assert
    imageDeletionResponse.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedUserDataWithoutImageContent!.Data.FirstOrDefault()!.profileImg.Should().BeNull();

    // Cleanup
    Directory.Delete(Constants.API_FILE_UPLOAD_OPTIONS_DIR, true);
  }
}
