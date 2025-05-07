using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class WishlistService(
  IWishlistRepository wishlistRepository,
  IUserRepository userRepository,
  IBookRepository bookRepository,
  IDateTimeService dateTimeService
) : IWishlistService
{
  private readonly IWishlistRepository _wishlistRepository = wishlistRepository;
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IBookRepository _bookRepository = bookRepository;
  private readonly IDateTimeService _dateTimeService = dateTimeService;

  public async Task<PaginatedListEnvelope<WishlistResponse>> GetWishlistAsync(
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

    var wishlistResponse = await _wishlistRepository.GetWishlistByUsernameAsync(
      pageSize!.Value,
      pageNumber!.Value,
      username
    );

    return wishlistResponse;
  }

  public async Task<Envelope<WishlistByBookResponse>> GetWishlistByBookAsync(
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

    var wishlistResponse = await _wishlistRepository.GetWishlistByUsernameAndBookAsync(
      username,
      bookId
    );

    if (wishlistResponse is null)
    {
      throw new NotFoundException("No wishlist for the given book id was found.");
    }

    return wishlistResponse;
  }

  public async Task AddWishlistAsync(int userId, string username, int bookId)
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

    // Tries to add wishlist to database
    var wishlist = new Wishlist()
    {
      UserId = userId,
      BookId = bookId,
      CreatedAt = _dateTimeService.Now(),
    };

    var wishlistEntity = await _wishlistRepository.AddWishlistAsync(wishlist);

    // The wishlist already exists on the database, remove previous data and insert the new one.
    if (wishlistEntity is null)
    {
      var removedWishlistEntity = await _wishlistRepository.RemoveWishlistAsync(
        wishlist.UserId,
        wishlist.BookId
      );

      if (removedWishlistEntity is null)
      {
        throw new Exception("Error removing wishlist from the database");
      }

      var newWishlistEntity = await _wishlistRepository.AddWishlistAsync(wishlist);

      if (newWishlistEntity is null)
      {
        throw new Exception("Error adding new wishlist from the database");
      }
    }
  }

  public async Task RemoveWishlistAsync(int userId, string username, int bookId)
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

    var response = await _wishlistRepository.RemoveWishlistAsync(userId, bookId);

    if (response is null)
    {
      throw new NotFoundException("No wishlist found.");
    }
  }
}
