using FluentAssertions;
using Moq;
using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class SettingServiceTests
{
  [Fact]
  public async Task GetSettingsAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockSettingRepository = new Mock<ISettingRepository>();

    mockSettingRepository
      .Setup(cr =>
        cr.GetSettingsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>()).Result
      )
      .Returns(new PaginatedListEnvelope<string?>([], 1, 1, 1));

    var settingService = new SettingService(mockSettingRepository.Object);

    // Act
    var response = await settingService.GetSettingsAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
  }
}
