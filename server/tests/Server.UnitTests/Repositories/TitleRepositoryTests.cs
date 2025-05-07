using FluentAssertions;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class TitleRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetTitlesAsync_NullParameter_ReturnsValidResponse()
  {
    // Arrange
    var book1 = new Book()
    {
      Id = 1,
      BookId = "",
      Isbn = "",
      CoverImg = "",
      Title = "a",
    };
    var book2 = new Book()
    {
      Id = 2,
      BookId = "",
      Isbn = "",
      CoverImg = "",
      Title = "b",
    };
    var book3 = new Book()
    {
      Id = 3,
      BookId = "",
      Isbn = "",
      CoverImg = "",
      Title = "c",
    };

    _context.Books.Add(book1);
    _context.Books.Add(book2);
    _context.Books.Add(book3);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var titleRepository = new TitleRepository(_context, autoMapper.mapper);

    var title1ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(book1))
      .FirstOrDefault();
    var title2ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(book2))
      .FirstOrDefault();
    var title3ListResponse = autoMapper
      .mapper.ProjectTo<string?>(QueryableUtils.MapToIQueryable(book3))
      .FirstOrDefault();

    IList<string?> titlesListResponse =
    [
      title1ListResponse,
      title2ListResponse,
      title3ListResponse,
    ];

    // Act
    var actual = await titleRepository.GetTitlesAsync(10, 1, null);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(3);
    actual.Data.Should().Equal(titlesListResponse);
  }

  [Fact]
  public async Task GetTitlesAsync_FilterTextParameter_ReturnsValidResponse()
  {
    // Arrange
    var filterText = "Test";

    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Isbn = "",
      CoverImg = "",
      Title = filterText,
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var titleRepository = new TitleRepository(_context, autoMapper.mapper);

    var titleListResponse = autoMapper.mapper.ProjectTo<string?>(
      QueryableUtils.MapToIQueryable(book)
    );

    // Act
    var actual = await titleRepository.GetTitlesAsync(10, 1, filterText);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(titleListResponse);
  }

  [Fact]
  public async Task GetTitlesAsync_FilterTextParameter_ReturnsInvalidResponse()
  {
    // Arrange
    var filterText = "Not valid";

    var book = new Book()
    {
      Id = 1,
      BookId = "",
      Isbn = "",
      CoverImg = "",
      Title = "Test",
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var titleRepository = new TitleRepository(_context, autoMapper.mapper);

    // Act
    var actual = await titleRepository.GetTitlesAsync(10, 1, filterText);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().HaveCount(0);
  }
}
