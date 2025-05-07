using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class BookApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetBooksAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var book = new Book
    {
      BookId = "",
      Title = "test",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/books");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookListResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(b => b.title!.Equals(book.Title)).Should().NotBeNull();
  }

  [Theory]
  [InlineData("rating")]
  [InlineData("date")]
  public async Task GetBooksAsync_ValidOrderByParameters_ReturnValidResponse(string orderBy)
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync($"/api/v1/books?orderBy={orderBy}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookListResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
  }

  [Fact]
  public async Task GetBooksAsync_GenreFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var book = new Book
    {
      BookId = "",
      Title = filter,
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var genre1 = new Genre() { Name = filter };
    var genre2 = new Genre() { Name = "b" };
    var genre3 = new Genre() { Name = "c" };

    genre1.Books.Add(book);

    _context.Genres.Add(genre1);
    _context.Genres.Add(genre2);
    _context.Genres.Add(genre3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/books?genre={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookListResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().title.Should().Be(filter);
  }

  [Fact]
  public async Task GetBooksAsync_TitleFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var book1 = new Book
    {
      BookId = "",
      Title = filter,
      Isbn = "",
      CoverImg = "",
    };
    var book2 = new Book
    {
      BookId = "",
      Title = "b",
      Isbn = "",
      CoverImg = "",
    };
    var book3 = new Book
    {
      BookId = "",
      Title = "c",
      Isbn = "",
      CoverImg = "",
    };

    _context.Books.Add(book1);
    _context.Books.Add(book2);
    _context.Books.Add(book3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/books?title={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookListResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().title.Should().Be(filter);
  }

  [Fact]
  public async Task GetBooksAsync_CharacterFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var book = new Book
    {
      BookId = "",
      Title = filter,
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var character1 = new Character() { Name = filter };
    var character2 = new Character() { Name = "b" };
    var character3 = new Character() { Name = "c" };

    character1.Books.Add(book);

    _context.Characters.Add(character1);
    _context.Characters.Add(character2);
    _context.Characters.Add(character3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/books?character={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookListResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().title.Should().Be(filter);
  }

  [Fact]
  public async Task GetBooksAsync_AwardFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var book = new Book
    {
      BookId = "",
      Title = filter,
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var award1 = new Award() { Name = filter };
    var award2 = new Award() { Name = "b" };
    var award3 = new Award() { Name = "c" };

    award1.Books.Add(book);

    _context.Awards.Add(award1);
    _context.Awards.Add(award2);
    _context.Awards.Add(award3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/books?award={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookListResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().title.Should().Be(filter);
  }

  [Fact]
  public async Task GetBooksAsync_SettingFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var book = new Book
    {
      BookId = "",
      Title = filter,
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    var setting1 = new Setting() { Name = filter };
    var setting2 = new Setting() { Name = "b" };
    var setting3 = new Setting() { Name = "c" };

    setting1.Books.Add(book);

    _context.Settings.Add(setting1);
    _context.Settings.Add(setting2);
    _context.Settings.Add(setting3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/books?setting={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookListResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().title.Should().Be(filter);
  }

  [Fact]
  public async Task GetBookByIdAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var book = new Book
    {
      BookId = "",
      Title = "test",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/1");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(b => b.title!.Equals(book.Title)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetBookByIdAsync_EmptyRating_ReturnValidResponse()
  {
    // Arrange
    var rating = new Rating();

    _context.Ratings.Add(rating);

    var book = new Book
    {
      BookId = "",
      Title = "test",
      Isbn = "",
      CoverImg = "",
    };

    book.Rating = rating;
    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/1");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(b => b.title!.Equals(book.Title)).Should().NotBeNull();
    parsedContent.Data.FirstOrDefault()!.rating.Should().BeNull();
  }

  [Fact]
  public async Task GetBookByIdAsync_ValidRating_ReturnValidResponse()
  {
    // Arrange
    var rating = new Rating()
    {
      Star5 = 1,
      StarsAverage = 5,
      StarsTotal = 1,
    };

    _context.Ratings.Add(rating);

    var book = new Book
    {
      BookId = "",
      Title = "test",
      Isbn = "",
      CoverImg = "",
    };

    book.Rating = rating;
    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/1");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(b => b.title!.Equals(book.Title)).Should().NotBeNull();
    parsedContent.Data.FirstOrDefault(b => b.rating!.starsTotal.Equals(1)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetBookByIdAsync_InvalidId_ReturnNotFoundResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task GetBookByBookIdAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var book = new Book
    {
      BookId = "bookId",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };
    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/bookId/bookId");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(b => b.bookId!.Equals(book.BookId)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetBookByBookIdAsync_EmptyRating_ReturnValidResponse()
  {
    // Arrange
    var rating = new Rating();

    _context.Ratings.Add(rating);

    var book = new Book
    {
      BookId = "bookId",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    book.Rating = rating;
    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/bookId/bookId");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(b => b.bookId!.Equals(book.BookId)).Should().NotBeNull();
    parsedContent.Data.FirstOrDefault()!.rating.Should().BeNull();
  }

  [Fact]
  public async Task GetBookByBookIdAsync_ValidRating_ReturnValidResponse()
  {
    // Arrange
    var rating = new Rating()
    {
      Star5 = 1,
      StarsAverage = 5,
      StarsTotal = 1,
    };

    _context.Ratings.Add(rating);

    var book = new Book
    {
      BookId = "bookId",
      Title = "",
      Isbn = "",
      CoverImg = "",
    };

    book.Rating = rating;
    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/bookId/bookId");
    var parsedContent = await response.Content.ReadFromJsonAsync<
      PaginatedListEnvelope<BookResponse>
    >();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(b => b.bookId!.Equals(book.BookId)).Should().NotBeNull();
    parsedContent.Data.FirstOrDefault(b => b.rating!.starsTotal.Equals(1)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetBookByBookIdAsync_InvalidId_ReturnNotFoundResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/books/bookId/bookId");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }
}
