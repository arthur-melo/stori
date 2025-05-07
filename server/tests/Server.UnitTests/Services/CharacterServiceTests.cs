using FluentAssertions;
using Moq;
using Server.API.Models;
using Server.API.Repositories.Interfaces;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class CharacterServiceTests
{
  [Fact]
  public async Task GetCharactersAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockCharacterRepository = new Mock<ICharacterRepository>();

    mockCharacterRepository
      .Setup(cr =>
        cr.GetCharactersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>()).Result
      )
      .Returns(new PaginatedListEnvelope<string?>([], 1, 1, 1));

    var characterService = new CharacterService(mockCharacterRepository.Object);

    // Act
    var response = await characterService.GetCharactersAsync(1, 1, "");

    // Assert
    response.Should().NotBeNull();
  }
}
