using System.Net.Http.Json;
using FluentAssertions;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class AwardApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetAwardsAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var award = new Award { Name = "Test" };
    _context.Awards.Add(award);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/awards");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(s => s!.Equals(award.Name)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetAwardsAsync_NameFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var award1 = new Award { Name = filter };
    var award2 = new Award { Name = "b" };
    var award3 = new Award { Name = "c" };

    _context.Awards.Add(award1);
    _context.Awards.Add(award2);
    _context.Awards.Add(award3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/awards?name={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().Should().Be(filter);
  }

  [Fact]
  public async Task GetAwardsAsync_EmptyDatabase_ReturnEmptyResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/awards");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }
}
