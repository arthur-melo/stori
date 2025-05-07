using Server.API.Models;
using Server.API.Models.Dtos.Responses;

namespace Server.API.Services.Interfaces;

public interface IBookService
{
  public Task<PaginatedListEnvelope<BookListResponse>> GetBooksAsync(
    int pageSize,
    int pageNumber,
    string orderBy,
    string? genre = null,
    string? title = null,
    string? character = null,
    string? award = null,
    string? setting = null
  );

  public Task<Envelope<BookResponse>> GetBookByIdAsync(int id);

  public Task<Envelope<BookResponse>> GetBookByBookIdAsync(string bookId);
}
