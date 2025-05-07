using FluentAssertions;
using Moq;
using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class TitleServiceTests
{
  [Fact]
  public async Task GetTitlesAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockTitleRepository = new Mock<ITitleRepository>();

    mockTitleRepository
      .Setup(cr => cr.GetTitlesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>()).Result)
      .Returns(new PaginatedListEnvelope<string?>([], 1, 1, 1));

    var titleService = new TitleService(mockTitleRepository.Object);

    // Act
    var response = await titleService.GetTitlesAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
  }
}
