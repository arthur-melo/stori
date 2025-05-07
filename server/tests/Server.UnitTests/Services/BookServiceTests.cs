using FluentAssertions;
using Moq;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Repositories.Interfaces;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class BookServiceTests
{
  [Fact]
  public async Task GetBooksAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockBookRepository = new Mock<IBookRepository>();

    mockBookRepository
      .Setup(br =>
        br.GetBooksAsync(
          It.IsAny<int>(),
          It.IsAny<int>(),
          It.IsAny<string>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>(),
          It.IsAny<string?>()
        ).Result
      )
      .Returns(new PaginatedListEnvelope<BookListResponse>([], 1, 1, 1));

    var bookService = new BookService(mockBookRepository.Object);

    // Act
    var response = await bookService.GetBooksAsync(1, 1, "", null, null, null, null, null);

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetBookByIdAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockBookRepository = new Mock<IBookRepository>();

    mockBookRepository
      .Setup(br => br.GetBookByIdAsync(It.IsAny<int>()).Result)
      .Returns(new PaginatedListEnvelope<BookResponse>(new List<BookResponse>(), 1, 1, 1));

    var bookService = new BookService(mockBookRepository.Object);

    // Act
    var response = await bookService.GetBookByIdAsync(1);

    // Assert
    response.Should().NotBeNull();
  }

  [Fact]
  public async Task GetBookByBookIdAsync_ValidParameters_ReturnsValidResponse()
  {
    // Arrange
    var mockBookRepository = new Mock<IBookRepository>();

    mockBookRepository
      .Setup(br => br.GetBookByBookIdAsync(It.IsAny<string>()).Result)
      .Returns(new PaginatedListEnvelope<BookResponse>(new List<BookResponse>(), 1, 1, 1));

    var bookService = new BookService(mockBookRepository.Object);

    // Act
    var response = await bookService.GetBookByBookIdAsync("");

    // Assert
    response.Should().NotBeNull();
  }
}
