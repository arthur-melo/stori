using FluentAssertions;
using Moq;
using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class GenreServiceTests
{
  [Fact]
  public async Task GetGenresAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockGenreRepository = new Mock<IGenreRepository>();

    mockGenreRepository
      .Setup(cr => cr.GetGenresAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>()).Result)
      .Returns(new PaginatedListEnvelope<string?>([], 1, 1, 1));

    var genreService = new GenreService(mockGenreRepository.Object);

    // Act
    var response = await genreService.GetGenresAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
  }
}
