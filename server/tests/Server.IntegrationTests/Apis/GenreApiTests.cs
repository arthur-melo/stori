using System.Net.Http.Json;
using FluentAssertions;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class GenreApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetGenresAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var genre = new Genre { Name = "Test" };
    _context.Genres.Add(genre);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/genres");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(s => s!.Equals(genre.Name)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetGenresAsync_NameFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var genre1 = new Genre { Name = filter };
    var genre2 = new Genre { Name = "b" };
    var genre3 = new Genre { Name = "c" };

    _context.Genres.Add(genre1);
    _context.Genres.Add(genre2);
    _context.Genres.Add(genre3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/genres?name={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().Should().Be(filter);
  }

  [Fact]
  public async Task GetGenresAsync_EmptyDatabase_ReturnEmptyResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/genres");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }
}
