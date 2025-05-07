using System.Net.Http.Json;
using FluentAssertions;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class CharacterApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetCharactersAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var character = new Character { Name = "Test" };
    _context.Characters.Add(character);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/characters");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(s => s!.Equals(character.Name)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetCharacterAsync_NameFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var character1 = new Character { Name = filter };
    var character2 = new Character { Name = "b" };
    var character3 = new Character { Name = "c" };

    _context.Characters.Add(character1);
    _context.Characters.Add(character2);
    _context.Characters.Add(character3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/characters?name={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().Should().Be(filter);
  }

  [Fact]
  public async Task GetCharactersAsync_EmptyDatabase_ReturnEmptyResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/characters");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }
}
