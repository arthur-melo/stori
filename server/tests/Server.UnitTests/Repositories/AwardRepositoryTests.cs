using FluentAssertions;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class AwardRepositoryTests() : BaseTests
{
  [Fact]
  public async Task GetAwardsAsync_NullParameter_ReturnsValidResponse()
  {
    // Arrange
    var pageSize = 10;
    var pageNumber = 1;

    _context.Awards.Add(new Award() { Id = 1, Name = "a" });
    _context.Awards.Add(new Award() { Id = 2, Name = "b" });
    _context.Awards.Add(new Award() { Id = 3, Name = "c" });

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();

    var awardRepository = new AwardRepository(_context, autoMapper.mapper);

    // Act
    var actual = await awardRepository.GetAwardsAsync(pageSize, pageNumber, null);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal("a", "b", "c");
  }

  [Fact]
  public async Task GetAwardsAsync_TextFilter_ReturnsValidResponse()
  {
    // Arrange
    var pageSize = 10;
    var pageNumber = 1;

    _context.Awards.Add(new Award() { Id = 1, Name = "a" });
    _context.Awards.Add(new Award() { Id = 2, Name = "b" });
    _context.Awards.Add(new Award() { Id = 3, Name = "c" });

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();

    var awardRepository = new AwardRepository(_context, autoMapper.mapper);
    // Act
    var actual = await awardRepository.GetAwardsAsync(pageSize, pageNumber, "a");

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal("a");
  }

  [Fact]
  public async Task GetAwardsAsync_NotFoundTextFilter_ReturnsEmptyList()
  {
    // Arrange
    var pageSize = 10;
    var pageNumber = 1;

    _context.Awards.Add(new Award() { Id = 1, Name = "a" });
    _context.Awards.Add(new Award() { Id = 2, Name = "b" });
    _context.Awards.Add(new Award() { Id = 3, Name = "c" });

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();

    var awardRepository = new AwardRepository(_context, autoMapper.mapper);

    // Act
    var actual = await awardRepository.GetAwardsAsync(pageSize, pageNumber, "d");

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }
}
