using FluentAssertions;
using Moq;
using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class AwardServicesTests
{
  [Fact]
  public async Task GetAwardsAsync_NullParameter_ReturnsValidResponse()
  {
    // Arrange
    var pageSize = 10;
    var pageNumber = 1;
    var totalItems = 1;
    List<string> stubList = [];

    var stubPaginatedListEnvelope = new Mock<PaginatedListEnvelope<string>>(
      stubList,
      pageSize,
      pageNumber,
      totalItems
    );
    var mockAwardRepository = new Mock<IAwardRepository>();

    mockAwardRepository
      .Setup(ar => ar.GetAwardsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>()).Result)
      .Returns(stubPaginatedListEnvelope.Object!);

    var awardService = new AwardService(mockAwardRepository.Object);

    // Act
    var actual = await awardService.GetAwardsAsync(pageSize, pageNumber, null);

    // Assert
    actual.Should().NotBeNull();
  }
}
