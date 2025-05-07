using FluentAssertions;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class SettingRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetSettingsAsync_NullParameter_ReturnsValidResponse()
  {
    // Arrange
    var pageSize = 10;
    var pageNumber = 1;

    _context.Settings.Add(new Setting() { Id = 1, Name = "a" });
    _context.Settings.Add(new Setting() { Id = 2, Name = "b" });
    _context.Settings.Add(new Setting() { Id = 3, Name = "c" });

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();

    var settingRepository = new SettingRepository(_context, autoMapper.mapper);

    // Act
    var actual = await settingRepository.GetSettingsAsync(pageSize, pageNumber, null);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal("a", "b", "c");
  }

  [Fact]
  public async Task GetSettingsAsync_TextFilter_ReturnsValidResponse()
  {
    // Arrange
    var pageSize = 10;
    var pageNumber = 1;

    _context.Settings.Add(new Setting() { Id = 1, Name = "a" });
    _context.Settings.Add(new Setting() { Id = 2, Name = "b" });
    _context.Settings.Add(new Setting() { Id = 3, Name = "c" });

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();

    var settingRepository = new SettingRepository(_context, autoMapper.mapper);
    // Act
    var actual = await settingRepository.GetSettingsAsync(pageSize, pageNumber, "a");

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal("a");
  }

  [Fact]
  public async Task GetSettingsAsync_NotFoundTextFilter_ReturnsEmptyList()
  {
    // Arrange
    var pageSize = 10;
    var pageNumber = 1;

    _context.Settings.Add(new Setting() { Id = 1, Name = "a" });
    _context.Settings.Add(new Setting() { Id = 2, Name = "b" });
    _context.Settings.Add(new Setting() { Id = 3, Name = "c" });

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();

    var settingRepository = new SettingRepository(_context, autoMapper.mapper);

    // Act
    var actual = await settingRepository.GetSettingsAsync(pageSize, pageNumber, "d");

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }
}
