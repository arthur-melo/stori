using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Repositories.Interfaces;

public interface IReadlistRepository
{
  public Task<PaginatedListEnvelope<ReadlistResponse>> GetReadlistByUsernameAsync(
    int pageSize,
    int pageNumber,
    string username
  );

  public Task<Envelope<ReadlistByBookResponse>?> GetReadlistByUsernameAndBookAsync(
    string username,
    int bookId
  );
  public Task<Readlist?> AddReadlistAsync(Readlist readlist);

  public Task<Readlist?> RemoveReadlistAsync(int userId, int bookId);
}
