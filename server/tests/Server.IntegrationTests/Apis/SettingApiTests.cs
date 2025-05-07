using System.Net.Http.Json;
using FluentAssertions;
using Server.API.Models;
using Server.API.Models.Entities;
using Server.IntegrationTests.Helpers;

namespace Server.IntegrationTests.Apis;

public class SettingApiTests(ApiFactory webApplicationFactory) : BaseTests(webApplicationFactory)
{
  [Fact]
  public async Task GetSettingsAsync_ValidParameters_ReturnValidResponse()
  {
    // Arrange
    var setting = new Setting { Name = "Test" };
    _context.Settings.Add(setting);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync("/api/v1/settings");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.FirstOrDefault(s => s!.Equals(setting.Name)).Should().NotBeNull();
  }

  [Fact]
  public async Task GetSettingsAsync_NameFilterParameter_ReturnValidResponse()
  {
    // Arrange
    var filter = "a";

    var setting1 = new Setting { Name = filter };
    var setting2 = new Setting { Name = "b" };
    var setting3 = new Setting { Name = "c" };

    _context.Settings.Add(setting1);
    _context.Settings.Add(setting2);
    _context.Settings.Add(setting3);

    await _context.SaveChangesAsync();

    // Act
    var response = await _httpClient.GetAsync($"/api/v1/settings?name={filter}");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(1);
    parsedContent.Data.First().Should().Be(filter);
  }

  [Fact]
  public async Task GetSettingsAsync_EmptyDatabase_ReturnEmptyResponse()
  {
    // Arrange
    // Act
    var response = await _httpClient.GetAsync("/api/v1/settings");
    var parsedContent = await response.Content.ReadFromJsonAsync<PaginatedListEnvelope<string?>>();

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    parsedContent.Should().NotBeNull();
    parsedContent!.Data.Should().HaveCount(0);
  }
}
