using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class UserRatingApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetUserRatingAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";

    var user = new User()
    {
      Email = "",
      Name = "",
      Password = "",
      Username = username,
      CreatedAt = DateTime.Now,
    };

    _context.Users.Add(user);

    var book = new Book()
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var userRating = new UserRating { CreatedAt = DateTime.Now, Rating = 5 };

    userRating.User = user;
    userRating.Book = book;

    _context.UserRatings.Add(userRating);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/user_ratings/{username}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<UserRatingResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent!.Data.FirstOrDefault()!.rating.Should().Be(userRating.Rating);
  }

  [Fact]
  public async Task GetUserRatingAsync_NoUserRating_ReturnValidResponse()
  {
    // Arrange
    var username = "username";

    var user = new User()
    {
      Email = "",
      Name = "",
      Password = "",
      Username = username,
      CreatedAt = DateTime.Now,
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/user_ratings/{username}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<UserRatingResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }

  [Fact]
  public async Task GetUserRatingByBookAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";

    var user = new User()
    {
      Email = "",
      Name = "",
      Password = "",
      Username = username,
      CreatedAt = DateTime.Now,
    };

    _context.Users.Add(user);

    var book = new Book()
    {
      BookId = "1",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var userRating = new UserRating { CreatedAt = DateTime.Now, Rating = 5 };

    userRating.User = user;
    userRating.Book = book;

    _context.UserRatings.Add(userRating);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/user_ratings/{username}/{book.Id}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      Envelope<UserRatingByBookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent!.Data.FirstOrDefault()!.rating.Should().Be(userRating.Rating);
  }

  [Fact]
  public async Task GetUserRatingByBookAsync_NoRating_ReturnNotFoundResponse()
  {
    // Arrange
    var username = "username";

    var user = new User()
    {
      Email = "",
      Name = "",
      Password = "",
      Username = username,
      CreatedAt = DateTime.Now,
    };

    _context.Users.Add(user);

    var book = new Book()
    {
      BookId = "1",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();
    // Act
    var response = await _httpClient.GetAsync(
      $"/api/v1/user_ratings/{user.Username}/{book.BookId}"
    );

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task AddUserRatingAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";
    var bookRating = 5;

    var book = new Book()
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var rating = new Rating { Book = book };

    _context.Ratings.Add(rating);

    await _context.SaveChangesAsync();

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var httpAddUserRatingRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new UserRatingRequestBody(bookRating)),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/user_ratings/{username}/1"),
    };

    httpAddUserRatingRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    // Act
    var response = await _httpClient.SendAsync(httpAddUserRatingRequest);

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299

    response
      .Headers.Where(h => h.Key.Equals("Location"))
      .SelectMany(h => h.Value)
      .FirstOrDefault()
      .Should()
      .Be($"/api/v1/user_ratings/{username}");

    _context.UserRatings.Should().HaveCount(1);
    _context.UserRatings.FirstOrDefault()!.Rating.Should().Be(bookRating);
  }

  [Fact]
  public async Task AddUserRatingAsync_ChangeRating_ReturnValidResponse()
  {
    // Arrange
    var username = "username";
    var newBookRating = 5;

    var book = new Book()
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var rating = new Rating { Book = book };

    _context.Ratings.Add(rating);

    await _context.SaveChangesAsync();

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var httpInitialAddUserRatingRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new UserRatingRequestBody(1)),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/user_ratings/{username}/1"),
    };

    httpInitialAddUserRatingRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    var httpModifiedAddUserRatingRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new UserRatingRequestBody(newBookRating)),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/user_ratings/{username}/1"),
    };

    httpModifiedAddUserRatingRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    // Act
    var initialAddUserRatingResponse = await _httpClient.SendAsync(httpInitialAddUserRatingRequest);
    var modifiedAddUserRatingResponse = await _httpClient.SendAsync(
      httpModifiedAddUserRatingRequest
    );

    // Assert
    initialAddUserRatingResponse.EnsureSuccessStatusCode(); // Status Code 200-299
    modifiedAddUserRatingResponse.EnsureSuccessStatusCode(); // Status Code 200-299

    modifiedAddUserRatingResponse
      .Headers.Where(h => h.Key.Equals("Location"))
      .SelectMany(h => h.Value)
      .FirstOrDefault()
      .Should()
      .Be($"/api/v1/user_ratings/{username}");

    _context.UserRatings.Should().HaveCount(1);
    _context.UserRatings.FirstOrDefault()!.Rating.Should().Be(newBookRating);
  }

  [Fact]
  public async Task RemoveUserRatingAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var username = "username";
    var bookRating = 5;

    var book = new Book()
    {
      BookId = "",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book);

    var rating = new Rating { Book = book };

    _context.Ratings.Add(rating);

    await _context.SaveChangesAsync();

    var userToken = await Utils.CreateUserAndGetTokenAsync(
      _httpClient,
      username,
      "example@email.com"
    );

    var httpAddUserRatingRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(
        JsonSerializer.Serialize(new UserRatingRequestBody(bookRating)),
        Encoding.UTF8,
        "application/json"
      ),
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/user_ratings/{username}/1"),
    };

    httpAddUserRatingRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    var httpRemoveUserRatingRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/user_ratings/{username}/1"),
    };

    httpRemoveUserRatingRequest.Headers.Add(
      "Authorization",
      $"Bearer {userToken!.accessToken!.token}"
    );

    // Act
    var addUserRatingResponse = await _httpClient.SendAsync(httpAddUserRatingRequest);
    var removeUserRatingResponse = await _httpClient.SendAsync(httpRemoveUserRatingRequest);

    // Assert
    addUserRatingResponse.EnsureSuccessStatusCode(); // Status Code 200-299
    removeUserRatingResponse.EnsureSuccessStatusCode();

    _context.UserRatings.Should().HaveCount(0);
  }
}
