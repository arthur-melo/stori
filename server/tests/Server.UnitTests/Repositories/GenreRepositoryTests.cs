using FluentAssertions;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class GenreRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetGenresAsync_NullParameter_ReturnsValidResponse()
  {
    // Arrange
    var genre1 = new Genre() { Id = 1, Name = "a" };
    var genre2 = new Genre() { Id = 2, Name = "b" };
    var genre3 = new Genre() { Id = 3, Name = "c" };

    _context.Genres.Add(genre1);
    _context.Genres.Add(genre2);
    _context.Genres.Add(genre3);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var genreRepository = new GenreRepository(_context, autoMapper.mapper);

    var genre1ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(genre1))
      .FirstOrDefault();
    var genre2ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(genre2))
      .FirstOrDefault();
    var genre3ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(genre3))
      .FirstOrDefault();

    IList<string?> genresListResponse =
    [
      genre1ListResponse,
      genre2ListResponse,
      genre3ListResponse,
    ];

    // Act
    var actual = await genreRepository.GetGenresAsync(10, 1, null);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(3);
    actual.Data.Should().Equal(genresListResponse);
  }

  [Fact]
  public async Task GetGenresAsync_FilterTextParameter_ReturnsValidResponse()
  {
    // Arrange
    var filterText = "Test";

    var genre = new Genre() { Id = 1, Name = filterText };

    _context.Genres.Add(genre);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var genreRepository = new GenreRepository(_context, autoMapper.mapper);

    var genreListResponse = autoMapper.mapper.ProjectTo<string?>(
      QueryableUtils.MapToIQueryable(genre)
    );

    // Act
    var actual = await genreRepository.GetGenresAsync(10, 1, filterText);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(genreListResponse);
  }

  [Fact]
  public async Task GetGenresAsync_FilterTextParameter_ReturnsInvalidResponse()
  {
    // Arrange
    var filterText = "Not valid";

    var genre = new Genre() { Id = 1, Name = "Test" };

    _context.Genres.Add(genre);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var genreRepository = new GenreRepository(_context, autoMapper.mapper);

    // Act
    var actual = await genreRepository.GetGenresAsync(10, 1, filterText);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }
}
