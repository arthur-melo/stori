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

public class WishlistApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetWishlistByBookAsync_ValidParameters_ReturnValidResponse()
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

    var wishlist = new Wishlist { CreatedAt = DateTime.Now, BookId = 1 };

    wishlist.User = user;
    wishlist.Book = book;

    _context.Wishlists.Add(wishlist);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/wishlists/{username}/{book.Id}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      Envelope<WishlistByBookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent!.Data.FirstOrDefault()!.book.id.Should().Be(wishlist.BookId);
  }

  [Fact]
  public async Task GetWishlistByBookAsync_NoRating_ReturnNotFoundResponse()
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
    var response = await _httpClient.GetAsync($"/api/v1/wishlists/{user.Username}/{book.BookId}");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task RemoveWishlistAsync_InvalidToken_ReturnErrorResponse()
  {
    // Arrange
    var httpRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/wishlists/username/1"),
    };

    // Act
    var response = await _httpClient.SendAsync(httpRequest);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task RemoveWishlistAsync_ValidParameters_ReturnValidResponse()
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
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/wishlists/{username}/1"),
    };

    httpCreateRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    // Act
    var createWishlistResponse = await _httpClient.SendAsync(httpCreateRequest);

    var httpRemoveRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/wishlists/{username}/1"),
    };

    httpRemoveRequest.Headers.Add("Authorization", $"Bearer {userToken!.accessToken!.token}");

    var removeWishlistResponse = await _httpClient.SendAsync(httpRemoveRequest);

    // Assert
    createWishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
    removeWishlistResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

    _context.Wishlists.Should().HaveCount(0);
  }

  [Fact]
  public async Task AddWishlistAsync_InvalidToken_ReturnErrorResponse()
  {
    // Arrange
    var httpRequest = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      RequestUri = new Uri(_httpClient.BaseAddress!, "/api/v1/wishlists/username/1"),
    };

    // Act
    var response = await _httpClient.SendAsync(httpRequest);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task AddWishlistAsync_ValidParameters_ReturnValidResponse()
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
      RequestUri = new Uri(_httpClient.BaseAddress!, $"/api/v1/wishlists/{username}/1"),
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
      .Be($"/api/v1/wishlists/{username}");

    _context.Wishlists.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetWishlistAsync_ValidParameters_ReturnValidResponse()
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

    var wishlist = new Wishlist { CreatedAt = DateTime.UtcNow };

    wishlist.Book = book;
    wishlist.User = user;

    _context.Wishlists.Add(wishlist);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/wishlists/{username}");

    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<WishlistResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
  }

  [Fact]
  public async Task GetWishlistAsync_InvalidUsername_ReturnErrorResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/wishlists/username");
    var parsedContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    parsedContent.Should().NotBeNull();
    parsedContent!.Type.Should().Be(nameof(NotFoundException));
  }

  [Fact]
  public async Task GetWishlistAsync_UserWithNoWishlistItems_ReturnEmptyResponse()
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
    var response = await _httpClient.GetAsync($"/api/v1/wishlists/{username}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<WishlistResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }
}
