using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class ReadlistService(
  IReadlistRepository readlistRepository,
  IUserRepository userRepository,
  IBookRepository bookRepository,
  IDateTimeService dateTimeService
) : IReadlistService
{
  private readonly IReadlistRepository _readlistRepository = readlistRepository;
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IBookRepository _bookRepository = bookRepository;
  private readonly IDateTimeService _dateTimeService = dateTimeService;

  public async Task<PaginatedListEnvelope<ReadlistResponse>> GetReadlistAsync(
    int? pageSize,
    int? pageNumber,
    string username
  )
  {
    var isUsernameInUse = await _userRepository.IsUsernameInUseAsync(username);

    if (!isUsernameInUse)
    {
      throw new NotFoundException("Invalid username.");
    }

    var readlistResponse = await _readlistRepository.GetReadlistByUsernameAsync(
      pageSize!.Value,
      pageNumber!.Value,
      username
    );

    return readlistResponse;
  }

  public async Task<Envelope<ReadlistByBookResponse>> GetReadlistByBookAsync(
    string username,
    int bookId
  )
  {
    // Verifies that the given username is valid.
    var isUsernameInUse = await _userRepository.IsUsernameInUseAsync(username);

    if (!isUsernameInUse)
    {
      throw new NotFoundException("Invalid username.");
    }

    // Verifies that the given bookId is valid.
    var book = await _bookRepository.IsBookInDatabaseAsync(bookId);

    if (!book)
    {
      throw new NotFoundException("No book found.");
    }

    var readlistResponse = await _readlistRepository.GetReadlistByUsernameAndBookAsync(
      username,
      bookId
    );

    if (readlistResponse is null)
    {
      throw new NotFoundException("No readlist for the given book id was found.");
    }

    return readlistResponse;
  }

  public async Task AddReadlistAsync(int userId, string username, int bookId)
  {
    // Verifies that the given username exists, and it belongs to the right id.
    var user = await _userRepository.GetUserByIdAsync(userId);

    if (user is null)
    {
      throw new ValidationException("Invalid current user.");
    }

    var requestedUser = await _userRepository.GetUserByUsernameAsync(username);

    if (requestedUser is null)
    {
      throw new NotFoundException("Invalid user.");
    }

    if (user.Username != requestedUser.username)
    {
      throw new ValidationException("The given username does not match the current user.");
    }

    // Verifies that the given bookId is valid:
    var book = await _bookRepository.IsBookInDatabaseAsync(bookId);

    if (!book)
    {
      throw new NotFoundException("No book found.");
    }

    // Tries to add readlist to database
    var readlist = new Readlist()
    {
      UserId = userId,
      BookId = bookId,
      CreatedAt = _dateTimeService.Now(),
    };

    var readlistEntity = await _readlistRepository.AddReadlistAsync(readlist);

    // The readlist already exists on the database, remove previous data and insert the new one.
    if (readlistEntity is null)
    {
      var removedReadlistEntity = await _readlistRepository.RemoveReadlistAsync(
        readlist.UserId,
        readlist.BookId
      );

      if (removedReadlistEntity is null)
      {
        throw new Exception("Error removing readlist from the database");
      }

      var newReadlistEntity = await _readlistRepository.AddReadlistAsync(readlist);

      if (newReadlistEntity is null)
      {
        throw new Exception("Error adding new readlist from the database");
      }
    }
  }

  public async Task RemoveReadlistAsync(int userId, string username, int bookId)
  {
    // Verifies that the given username exists, and it belongs to the right id.
    var user = await _userRepository.GetUserByIdAsync(userId);

    if (user is null)
    {
      throw new NotFoundException("Invalid user.");
    }

    var requestedUser = await _userRepository.GetUserByUsernameAsync(username);

    if (requestedUser is null)
    {
      throw new NotFoundException("Invalid user.");
    }

    if (user.Username != username)
    {
      throw new ValidationException("The given username does not match the current user.");
    }

    // Verifies that the given bookId is valid:
    var book = await _bookRepository.IsBookInDatabaseAsync(bookId);

    if (!book)
    {
      throw new NotFoundException("No book found.");
    }

    var response = await _readlistRepository.RemoveReadlistAsync(userId, bookId);

    if (response is null)
    {
      throw new NotFoundException("No readlist found.");
    }
  }
}
