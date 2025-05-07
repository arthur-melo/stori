using Server.API.Exceptions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class ReviewService(
  IReviewRepository reviewRepository,
  IUserRepository userRepository,
  IBookRepository bookRepository,
  IDateTimeService dateTimeService
) : IReviewService
{
  private readonly IReviewRepository _reviewRepository = reviewRepository;
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IBookRepository _bookRepository = bookRepository;
  private readonly IDateTimeService _dateTimeService = dateTimeService;

  public async Task<PaginatedListEnvelope<ReviewResponse>> GetReviewByUsernameAsync(
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

    var reviewResponse = await _reviewRepository.GetReviewByUsernameAsync(
      pageSize!.Value,
      pageNumber!.Value,
      username
    );

    return reviewResponse;
  }

  public async Task<PaginatedListEnvelope<ReviewBookResponse>> GetReviewByBookAsync(
    int? pageSize,
    int? pageNumber,
    int bookId
  )
  {
    var book = await _bookRepository.IsBookInDatabaseAsync(bookId);

    if (!book)
    {
      throw new NotFoundException("Invalid book.");
    }

    var reviewResponse = await _reviewRepository.GetReviewByBookAsync(
      pageSize!.Value,
      pageNumber!.Value,
      bookId
    );

    return reviewResponse;
  }

  public async Task<string> AddReviewByBookAsync(int userId, int bookId, string text)
  {
    // Verifies that the given user exists.
    var isUserInDatabase = await _userRepository.IsUserInDatabaseAsync(userId);

    if (!isUserInDatabase)
    {
      throw new ValidationException("Invalid current user.");
    }

    // Verifies that the given bookId is valid:
    var isBookInDatabase = await _bookRepository.IsBookInDatabaseAsync(bookId);

    if (!isBookInDatabase)
    {
      throw new NotFoundException("Invalid book.");
    }

    // Tries to add rating to database
    var review = new Review()
    {
      UserId = userId,
      BookId = bookId,
      Text = text,
      CreatedAt = _dateTimeService.Now(),
    };

    var reviewEntity = await _reviewRepository.AddReviewAsync(review);

    if (reviewEntity is null)
    {
      throw new Exception("Error adding review to the database");
    }

    return reviewEntity.User.Username;
  }

  public async Task<string> PatchReviewByIdAsync(int userId, int reviewId, string text)
  {
    // Verifies that the given user exists.
    var user = await _userRepository.GetUserByIdAsync(userId);

    if (user is null)
    {
      throw new ValidationException("Invalid current user.");
    }

    // Verifies that the given bookId is valid:
    var review = await _reviewRepository.GetReviewByIdAsync(reviewId);

    if (review is null)
    {
      throw new NotFoundException("Invalid review.");
    }

    if (review.author.username != user.Username)
    {
      throw new ValidationException("The current user cannot edit a review from another user.");
    }

    var reviewEntity = await _reviewRepository.PatchReviewAsync(reviewId, text);

    if (reviewEntity is null)
    {
      throw new Exception("Error editing review to the database");
    }

    return user.Username;
  }

  public async Task RemoveReviewAsync(int userId, string username, int reviewId)
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

    var response = await _reviewRepository.RemoveReviewAsync(reviewId);

    if (response is null)
    {
      throw new NotFoundException("No review found.");
    }
  }
}
