using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class ReviewApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetReviewByUsernameAsync_ValidParameters_ReturnValidResponse()
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

    var book = new Book
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var review = new Review { CreatedAt = DateTime.UtcNow, Text = "" };

    review.Book = book;
    review.User = user;

    _context.Reviews.Add(review);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/reviews/{username}");

    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<ReviewResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetReviewByUsernameAsync_UserWithNoReviews_ReturnEmptyResponse()
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
    var response = await _httpClient.GetAsync($"/api/v1/reviews/{username}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<ReviewResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task GetReviewByUsernameAsync_InvalidUsername_ReturnErrorResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/reviews/username");
    var parsedContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    parsedContent.Should().NotBeNull();
    parsedContent!.Type.Should().Be(nameof(NotFoundException));
  }

  [Fact]
  public async Task RemoveReviewByBookAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var book = new Book
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var username = "username";

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var httpAddReviewRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new ReviewRequestNewCommentBody("text")),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/reviews/book/1"),
    };

    httpAddReviewRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    // Act
    var addReviewResponse = await _httpClient.SendAsync(httpAddReviewRequest);

    var httpRemoveRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      Content = new StringContent(
        JsonSerializer.Serialize(new ReviewRequestCommentBody(1)),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/reviews/{username}"),
    };

    httpRemoveRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    var removeReviewResponse = await _httpClient.SendAsync(httpRemoveRequest);

    // Assert
    addReviewResponse.EnsureSuccessStatusCode(); // Status Code 200-299
    removeReviewResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

    _context.Reviews.Should().HaveCount(0);
  }

  [Fact]
  public async Task RemoveReviewByBookAsync_InvalidToken_ReturnErrorResponse()
  {
    // Arrange
    var httpRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/reviews/username"),
    };

    // Act
    var response = await _httpClient.SendAsync(httpRequest);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task GetReviewByBookAsync_ValidParameters_ReturnValidResponse()
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

    var book = new Book
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var review = new Review { CreatedAt = DateTime.UtcNow, Text = "" };

    review.Book = book;
    review.User = user;

    _context.Reviews.Add(review);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/reviews/book/1");

    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<ReviewResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetReviewByBookAsync_BookWithNoReviews_ReturnEmptyResponse()
  {
    // Arrange
    var book = new Book
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/reviews/book/1");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<ReviewResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task GetReviewByBookAsync_InvalidBook_ReturnNotFoundResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/reviews/book/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task AddReviewByBookAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var book = new Book
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var username = "username";

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var httpAddReviewRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new ReviewRequestNewCommentBody("text")),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/reviews/book/1"),
    };

    httpAddReviewRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    // Act
    var response = await _httpClient.SendAsync(httpAddReviewRequest);

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299

    response
      .Headers.Where(h => h.Key.Equals("Location"))
      .SelectMany(h => h.Value)
      .FirstOrDefault()
      .Should()
      .Be($"/api/v1/reviews/{username}");

    _context.Reviews.Should().HaveCount(1);
  }

  [Fact]
  public async Task AddReviewByBookAsync_InvalidToken_ReturnErrorResponse()
  {
    // Arrange
    var httpRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/reviews/book"),
    };

    // Act
    var response = await _httpClient.SendAsync(httpRequest);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task PatchReviewByIdAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";
    var newText = "New text for review";

    var book = new Book
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var httpAddReviewRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new ReviewRequestNewCommentBody("initial text")),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/reviews/book/1"),
    };

    httpAddReviewRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    var httpReviewPatchRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Patch,
      Content = new StringContent(
        JsonSerializer.Serialize(new ReviewRequestNewCommentBody(newText)),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/reviews/patch/1"),
    };

    httpReviewPatchRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    // Act
    await _httpClient.SendAsync(httpAddReviewRequest);

    var patchResponse = await _httpClient.SendAsync(httpReviewPatchRequest);

    // Assert
    patchResponse.EnsureSuccessStatusCode(); // Status Code 200-299

    patchResponse
      .Headers.Where(h => h.Key.Equals("Location"))
      .SelectMany(h => h.Value)
      .FirstOrDefault()
      .Should()
      .Be($"/api/v1/reviews/{username}");

    _context.Reviews.Should().HaveCount(1);
    _context.Reviews.FirstOrDefault()!.Text.Should().Be(newText);
  }

  [Fact]
  public async Task PatchReviewByIdAsync_EditingAnotherUserReview_ReturnErrorResponse()
  {
    // Arrange
    var username = "username";

    var book = new Book
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var user1Token = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var user2Token = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      "username2",
      "example@email.com"
    );

    var httpAddReviewRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new ReviewRequestNewCommentBody("initial text")),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/reviews/book/1"),
    };

    httpAddReviewRequest.Headers.Add("Authorization", $"Bearer {user1Token!.accessToken!.token}");

    var httpReviewPatchRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Patch,
      Content = new StringContent(
        JsonSerializer.Serialize(new ReviewRequestNewCommentBody("")),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/reviews/patch/1"),
    };

    httpReviewPatchRequest.Headers.Add("Authorization", $"Bearer {user2Token!.accessToken!.token}");

    // Act
    await _httpClient.SendAsync(httpAddReviewRequest);

    var patchResponse = await _httpClient.SendAsync(httpReviewPatchRequest);

    // Assert
    patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }
}
