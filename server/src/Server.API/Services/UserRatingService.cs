using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class UserRatingService(
  IUserRatingRepository userRatingRepository,
  IUserRepository userRepository,
  IBookRepository bookRepository,
  IRatingService ratingService,
  IDateTimeService dateTimeService
) : IUserRatingService
{
  private readonly IUserRatingRepository _userRatingRepository = userRatingRepository;
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IBookRepository _bookRepository = bookRepository;
  private readonly IRatingService _ratingService = ratingService;
  private readonly IDateTimeService _dateTimeService = dateTimeService;

  public async Task<PaginatedListEnvelope<UserRatingResponse>> GetUserRatingAsync(
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

    var userRatingResponse = await _userRatingRepository.GetUserRatingByUsernameAsync(
      pageSize!.Value,
      pageNumber!.Value,
      username
    );

    return userRatingResponse;
  }

  public async Task<Envelope<UserRatingByBookResponse>?> GetUserRatingByBookAsync(
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

    var userRatingResponse = await _userRatingRepository.GetUserRatingByUsernameAndBookAsync(
      username,
      bookId
    );

    if (userRatingResponse is null)
    {
      throw new NotFoundException("No user rating for the given book id.");
    }

    return userRatingResponse;
  }

  public async Task<string> AddUserRatingAsync(int userId, string username, int bookId, int rating)
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

    // Tries to add rating to database
    var userRating = new UserRating()
    {
      UserId = userId,
      BookId = bookId,
      Rating = rating,
      CreatedAt = _dateTimeService.Now(),
    };

    var userRatingEntity = await _userRatingRepository.AddUserRatingAsync(userRating);

    // The rating already exists on the database, remove previous data and insert the new one.
    if (userRatingEntity is null)
    {
      var removedUserRatingEntity = await _userRatingRepository.RemoveUserRatingAsync(
        userRating.UserId,
        userRating.BookId
      );

      if (removedUserRatingEntity is null)
      {
        throw new Exception("Error removing userRating from the database");
      }

      // Delete from the book rating table the previous rating
      await _ratingService.DeleteBookRatingAsync(bookId, removedUserRatingEntity.Rating);

      var newUserRatingEntity = await _userRatingRepository.AddUserRatingAsync(userRating);

      if (newUserRatingEntity is null)
      {
        throw new Exception("Error adding new userRating from the database");
      }
    }

    // Update the current bookId rating with the new value
    await _ratingService.AddBookRatingAsync(bookId, rating);

    return user.Username;
  }

  public async Task RemoveUserRatingAsync(int userId, string username, int bookId)
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

    var removedUserRating = await _userRatingRepository.RemoveUserRatingAsync(userId, bookId);

    if (removedUserRating is null)
    {
      throw new NotFoundException("No user rating found.");
    }

    // Delete from the book rating table the previous rating
    await _ratingService.DeleteBookRatingAsync(bookId, removedUserRating.Rating);
  }
}
