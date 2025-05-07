using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class ReadlistApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetReadlistByBookAsync_ValidParameters_ReturnValidResponse()
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

    var readlist = new Readlist { CreatedAt = DateTime.Now, BookId = 1 };

    readlist.User = user;
    readlist.Book = book;

    _context.Readlists.Add(readlist);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/readlists/{username}/{book.Id}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      Envelope<ReadlistByBookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent!.Data.FirstOrDefault()!.book.id.Should().Be(readlist.BookId);
  }

  [Fact]
  public async Task GetReadlistByBookAsync_NoRating_ReturnNotFoundResponse()
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
    var response = await _httpClient.GetAsync($"/api/v1/readlists/{user.Username}/{book.BookId}");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task RemoveReadlistAsync_InvalidToken_ReturnErrorResponse()
  {
    // Arrange
    var httpRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/readlists/username/1"),
    };

    // Act
    var response = await _httpClient.SendAsync(httpRequest);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task RemoveReadlistAsync_ValidParameters_ReturnValidResponse()
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

    var httpCreateRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/readlists/{username}/1"),
    };

    httpCreateRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    // Act
    var createReadlistResponse = await _httpClient.SendAsync(httpCreateRequest);

    var httpRemoveRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/readlists/{username}/1"),
    };

    httpRemoveRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    var removeReadlistResponse = await _httpClient.SendAsync(httpRemoveRequest);

    // Assert
    createReadlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
    removeReadlistResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

    _context.Readlists.Should().HaveCount(0);
  }

  [Fact]
  public async Task AddReadlistAsync_InvalidToken_ReturnErrorResponse()
  {
    // Arrange
    var httpRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/readlists/username/1"),
    };

    // Act
    var response = await _httpClient.SendAsync(httpRequest);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task AddReadlistAsync_ValidParameters_ReturnValidResponse()
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

    var httpRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/readlists/{username}/1"),
    };

    httpRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    // Act
    var response = await _httpClient.SendAsync(httpRequest);

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    response
      .Headers.Where(h => h.Key.Equals("Location"))
      .SelectMany(h => h.Value)
      .FirstOrDefault()
      .Should()
      .Be($"/api/v1/readlists/{username}");

    _context.Readlists.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetReadlistAsync_ValidParameters_ReturnValidResponse()
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

    var readlist = new Readlist { CreatedAt = DateTime.UtcNow };

    readlist.Book = book;
    readlist.User = user;

    _context.Readlists.Add(readlist);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/readlists/{username}");

    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<ReadlistResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetReadlistAsync_InvalidUsername_ReturnErrorResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/readlists/username");
    var parsedContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    parsedContent.Should().NotBeNull();
    parsedContent!.Type.Should().Be(nameof(NotFoundException));
  }

  [Fact]
  public async Task GetReadlistAsync_UserWithNoReadlistItems_ReturnEmptyResponse()
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
    var response = await _httpClient.GetAsync($"/api/v1/readlists/{username}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<ReadlistResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }
}
