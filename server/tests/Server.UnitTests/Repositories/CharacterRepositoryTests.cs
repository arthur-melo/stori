using FluentAssertions;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class CharacterRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetCharactersAsync_NullParameter_ReturnsValidResponse()
  {
    // Arrange
    var character1 = new Character() { Id = 1, Name = "a" };
    var character2 = new Character() { Id = 2, Name = "b" };
    var character3 = new Character() { Id = 3, Name = "c" };

    _context.Characters.Add(character1);
    _context.Characters.Add(character2);
    _context.Characters.Add(character3);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var characterRepository = new CharacterRepository(_context, autoMapper.mapper);

    var character1ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(character1))
      .FirstOrDefault();
    var character2ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(character2))
      .FirstOrDefault();
    var character3ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(character3))
      .FirstOrDefault();

    IList<string?> charactersListResponse =
    [
      character1ListResponse,
      character2ListResponse,
      character3ListResponse,
    ];

    // Act
    var actual = await characterRepository.GetCharactersAsync(10, 1, null);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(3);
    actual.Data.Should().Equal(charactersListResponse);
  }

  [Fact]
  public async Task GetCharactersAsync_FilterTextParameter_ReturnsValidResponse()
  {
    // Arrange
    var filterText = "Test";

    var character = new Character() { Id = 1, Name = filterText };

    _context.Characters.Add(character);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var characterRepository = new CharacterRepository(_context, autoMapper.mapper);

    var characterListResponse = autoMapper.mapper.ProjectTo<string?>(
      QueryableUtils.MapToIQueryable(character)
    );

    // Act
    var actual = await characterRepository.GetCharactersAsync(10, 1, filterText);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(characterListResponse);
  }

  [Fact]
  public async Task GetCharactersAsync_FilterTextParameter_ReturnsInvalidResponse()
  {
    // Arrange
    var filterText = "Not valid";

    var character = new Character() { Id = 1, Name = "Test" };

    _context.Characters.Add(character);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var characterRepository = new CharacterRepository(_context, autoMapper.mapper);

    // Act
    var actual = await characterRepository.GetCharactersAsync(10, 1, filterText);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }
}
