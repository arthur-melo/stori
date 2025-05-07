using FluentAssertions;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class BookRepositoryTests : BaseTests
{
  private Book CreateBook(
    int id,
    int? publisherId = null,
    string? bookId = "",
    string? title = "",
    string? series = null,
    string? description = null,
    string? language = null,
    string? isbn = "",
    string? bookFormat = null,
    string? edition = null,
    int? pages = null,
    DateOnly? publishDate = null,
    string? coverImg = "",
    Rating? rating = null
  )
  {
    return new Book()
    {
      Id = id,
      Rating = rating ?? new Rating(),
      PublisherId = publisherId,
      BookId = bookId!,
      Title = title!,
      Series = series,
      Description = description,
      Language = language,
      Isbn = isbn!,
      BookFormat = bookFormat,
      Edition = edition,
      Pages = pages,
      PublishDate = publishDate ?? new DateOnly(),
      CoverImg = coverImg!,
    };
  }

  [Fact]
  public async Task GetBooksAsync_NullParameter_ReturnsValidResponse()
  {
    // Arrange
    var book = CreateBook(1);

    _context.Books.Add(book);
    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    var bookListResponse = autoMapper.mapper.ProjectTo<BookListResponse>(
      QueryableUtils.MapToIQueryable(book)
    );

    // Act
    var actual = await bookRepository.GetBooksAsync(10, 1, "rating", null, null, null, null, null);

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(bookListResponse);
  }

  [Theory]
  [InlineData("genre", null, null, null, null)]
  [InlineData(null, "title", null, null, null)]
  [InlineData(null, null, "character", null, null)]
  [InlineData(null, null, null, "award", null)]
  [InlineData(null, null, null, null, "setting")]
  public async Task GetBooksAsync_TextFilters_ReturnsValidResponse(
    string? genre,
    string? title,
    string? character,
    string? award,
    string? setting
  )
  {
    // Arrange
    var book1 = CreateBook(1);
    var book2 = CreateBook(2);
    var book3 = CreateBook(3);

    _context.Books.Add(book1);
    _context.Books.Add(book2);
    _context.Books.Add(book3);

    if (genre is not null)
    {
      var genreEntity = new Genre() { Id = 1, Name = genre };
      genreEntity.Books.Add(book1);
      _context.Genres.Add(genreEntity);
    }

    if (title is not null)
    {
      book1.Title = title;
    }

    if (character is not null)
    {
      var characterEntity = new Character() { Id = 1, Name = character };
      characterEntity.Books.Add(book1);
      _context.Characters.Add(characterEntity);
    }

    if (award is not null)
    {
      var awardEntity = new Award() { Id = 1, Name = award };
      awardEntity.Books.Add(book1);
      _context.Awards.Add(awardEntity);
    }

    if (setting is not null)
    {
      var settingEntity = new Setting() { Id = 1, Name = setting };
      settingEntity.Books.Add(book1);
      _context.Settings.Add(settingEntity);
    }

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    var bookListResponse = autoMapper.mapper.ProjectTo<BookListResponse>(
      QueryableUtils.MapToIQueryable(book1)
    );

    // Act
    var actual = await bookRepository.GetBooksAsync(
      10,
      1,
      "rating",
      genre,
      title,
      character,
      award,
      setting
    );

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(bookListResponse);
  }

  [Fact]
  public async Task GetBooksAsync_RatingDateTextFilter_ReturnsValidResponse()
  {
    // Arrange
    var orderByType = "date";

    var book1 = CreateBook(1, publishDate: new DateOnly(1, 1, 1));
    var book2 = CreateBook(2, publishDate: new DateOnly(1, 1, 2));
    var book3 = CreateBook(3, publishDate: new DateOnly(1, 1, 3));

    _context.Books.Add(book1);
    _context.Books.Add(book2);
    _context.Books.Add(book3);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    var book1ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book1))
      .First();
    var book2ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book2))
      .First();
    var book3ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book3))
      .First();

    // Most Recent first
    List<BookListResponse> bookListCollection =
    [
      book3ListResponse,
      book2ListResponse,
      book1ListResponse,
    ];

    // Act
    var actual = await bookRepository.GetBooksAsync(
      10,
      1,
      orderByType,
      null,
      null,
      null,
      null,
      null
    );

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(bookListCollection);
  }

  [Fact]
  public async Task GetBooksAsync_RatingTextFilter_ReturnsValidResponse()
  {
    // Arrange
    var orderByType = "rating";

    var rating1 = new Rating()
    {
      BookId = 1,
      StarsAverage = 1,
      StarsTotal = 1,
    };

    var rating2 = new Rating()
    {
      BookId = 2,
      StarsAverage = 2,
      StarsTotal = 2,
    };

    var rating3 = new Rating()
    {
      BookId = 3,
      StarsAverage = 3,
      StarsTotal = 3,
    };

    var book1 = CreateBook(1, rating: rating1);
    var book2 = CreateBook(2, rating: rating2);
    var book3 = CreateBook(3, rating: rating3);

    _context.Books.Add(book1);
    _context.Books.Add(book2);
    _context.Books.Add(book3);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    var book1ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book1))
      .First();
    var book2ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book2))
      .First();
    var book3ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book3))
      .First();

    // Best rated first
    List<BookListResponse> bookListCollection =
    [
      book3ListResponse,
      book2ListResponse,
      book1ListResponse,
    ];

    // Act
    var actual = await bookRepository.GetBooksAsync(
      10,
      1,
      orderByType,
      null,
      null,
      null,
      null,
      null
    );

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(bookListCollection);
  }

  [Fact]
  public async Task GetBooksAsync_OrderByDefaultTextFilter_ReturnsValidResponse()
  {
    // Arrange
    var orderByType = "anything else";

    var book1 = CreateBook(1);
    var book2 = CreateBook(2);
    var book3 = CreateBook(3);

    _context.Books.Add(book1);
    _context.Books.Add(book2);
    _context.Books.Add(book3);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    var book1ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book1))
      .First();
    var book2ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book2))
      .First();
    var book3ListResponse = autoMapper
      .mapper.ProjectTo<BookListResponse>(QueryableUtils.MapToIQueryable(book3))
      .First();

    // Most Recent first
    List<BookListResponse> bookListCollection =
    [
      book1ListResponse,
      book2ListResponse,
      book3ListResponse,
    ];

    // Act
    var actual = await bookRepository.GetBooksAsync(
      10,
      1,
      orderByType,
      null,
      null,
      null,
      null,
      null
    );

    // Assert
    actual.Should().NotBeNull();
    actual.Data.Should().Equal(bookListCollection);
  }

  [Fact]
  public async Task GetBookByIdAsync_GetValidBook_ReturnsValidResponse()
  {
    // Arrange
    var id = 1;

    var book = CreateBook(id);

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    // Act
    var actual = await bookRepository.GetBookByIdAsync(id);

    // Assert
    actual!.Data.Should().NotBeNull();
    actual!.Data.Should().HaveCount(1);
    actual!.Data.First().id.Should().Be(id);
  }

  [Fact]
  public async Task GetBookByIdAsync_GetInvalidBook_ReturnsNullResponse()
  {
    // Arrange
    var id = 2;

    var book1 = CreateBook(1);

    _context.Books.Add(book1);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    // Act
    var actual = await bookRepository.GetBookByIdAsync(id);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetBookByBookIdAsync_GetValidBook_ReturnsValidResponse()
  {
    // Arrange
    var bookId = "bookId";

    var book = CreateBook(1, null, bookId);

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    // Act
    var actual = await bookRepository.GetBookByBookIdAsync(bookId);

    // Assert
    actual!.Data.Should().NotBeNull();
    actual!.Data.Should().HaveCount(1);
    actual!.Data.First().bookId.Should().Be(bookId);
  }

  [Fact]
  public async Task GetBookByBookIdAsync_GetInvalidBook_ReturnsNullResponse()
  {
    // Arrange
    var book1 = CreateBook(1, null, "bookId");

    _context.Books.Add(book1);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    // Act
    var actual = await bookRepository.GetBookByBookIdAsync("invalid");

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task IsBookInDatabaseAsync_GetValidBook_ReturnsValidResponse()
  {
    // Arrange
    var bookId = 1;

    var book = CreateBook(bookId);

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    var book1ListResponse = autoMapper.mapper.ProjectTo<BookListResponse>(
      QueryableUtils.MapToIQueryable(book)
    );

    // Act
    var actual = await bookRepository.IsBookInDatabaseAsync(bookId);

    // Assert
    actual.Should().BeTrue();
  }

  [Fact]
  public async Task IsBookInDatabaseAsync_GetInvalidBook_ReturnsInvalidResponse()
  {
    // Arrange
    var bookId = 2;

    var book = CreateBook(1);

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var bookRepository = new BookRepository(_context, autoMapper.mapper);

    // Act
    var actual = await bookRepository.IsBookInDatabaseAsync(bookId);

    // Assert
    actual.Should().BeFalse();
  }
}
