using Server.API.Models;
using Server.API.Models.Dtos.Responses;

namespace Server.API.Services.Interfaces;

public interface IReadlistService
{
  public Task<PaginatedListEnvelope<ReadlistResponse>> GetReadlistAsync(
    int? pageSize,
    int? pageNumber,
    string username
  );

  public Task<Envelope<ReadlistByBookResponse>> GetReadlistByBookAsync(string username, int bookId);

  public Task AddReadlistAsync(int userId, string username, int bookId);

  public Task RemoveReadlistAsync(int userId, string username, int bookId);
}
