using Server.API.Models;
using Server.API.Models.Dtos.Responses;

namespace Server.API.Repositories.Interfaces;

public interface IBookRepository
{
  public Task<PaginatedListEnvelope<BookListResponse>> GetBooksAsync(
    int pageSize,
    int pageNumber,
    string orderBy,
    string? genre,
    string? title,
    string? character,
    string? award,
    string? setting
  );
  public Task<Envelope<BookResponse>?> GetBookByIdAsync(int id);
  public Task<Envelope<BookResponse>?> GetBookByBookIdAsync(string bookId);
  public Task<bool> IsBookInDatabaseAsync(int bookId);
}
