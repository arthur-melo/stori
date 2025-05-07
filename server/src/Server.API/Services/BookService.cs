using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
  private readonly IBookRepository _bookRepository = bookRepository;

  public async Task<PaginatedListEnvelope<BookListResponse>> GetBooksAsync(
    int pageSize,
    int pageNumber,
    string orderBy,
    string? genre,
    string? title,
    string? character,
    string? award,
    string? setting
  )
  {
    var books = await _bookRepository.GetBooksAsync(
      pageSize,
      pageNumber,
      orderBy,
      genre,
      title,
      character,
      award,
      setting
    );

    return books;
  }

  public async Task<Envelope<BookResponse>> GetBookByIdAsync(int id)
  {
    var response = await _bookRepository.GetBookByIdAsync(id);

    if (response is null)
    {
      throw new NotFoundException("Invalid id.");
    }
    return response;
  }

  public async Task<Envelope<BookResponse>> GetBookByBookIdAsync(string bookId)
  {
    var response = await _bookRepository.GetBookByBookIdAsync(bookId);

    if (response is null)
    {
      throw new NotFoundException("Invalid book id.");
    }
    return response;
  }
}
