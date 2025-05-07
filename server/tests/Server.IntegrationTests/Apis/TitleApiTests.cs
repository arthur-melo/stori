using System.Net.Http.Json;
using FluentAssertions;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class TitleApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetTitlesAsync_ValidParameters_ReturnValidResponse()
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
    var response = await _httpClient.GetAsync("/api/v1/titles");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(s => s!.Equals(book.Title)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetTitleAsync_NameFilterParameter_ReturnValidResponse()
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
    var response = await _httpClient.GetAsync($"/api/v1/titles?name={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().Should().Be(filter);
  }

  [Fact]
  public async Task GetTitlesAsync_EmptyDatabase_ReturnEmptyResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/titles");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }
}
