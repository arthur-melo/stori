using FluentAssertions;
using Moq;
using Server.API.Exceptions;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class RatingServiceTests
{
  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public async Task AddBookRatingAsync_DefaultRatingParameters_ReturnsValidResponse(int rating)
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(() =>
      {
        var newRating = new Rating();
        switch (rating)
        {
          case 1:
            newRating.Star1 = 1;
            break;
          case 2:
            newRating.Star2 = 1;
            break;
          case 3:
            newRating.Star3 = 1;
            break;
          case 4:
            newRating.Star4 = 1;
            break;
          case 5:
            newRating.Star5 = 1;
            break;
          default:
            throw new Exception("Invalid rating value");
        }

        return newRating;
      });

    var ratingService = new RatingService(mockRatingRepository.Object);

    Func<Rating, bool> validator = r =>
    {
      var isValid = false;

      isValid = rating switch
      {
        1 => r.Star1 == 2,
        2 => r.Star2 == 2,
        3 => r.Star3 == 2,
        4 => r.Star4 == 2,
        5 => r.Star5 == 2,
        _ => throw new Exception("Invalid rating value"),
      };

      return isValid;
    };

    // Act
    await ratingService.AddBookRatingAsync(1, rating);

    // Assert
    mockRatingRepository.Verify(
      rr => rr.SaveBookRatingAsync(It.Is<Rating>(r => validator(r))),
      Times.Once()
    );
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public async Task AddBookRatingAsync_PositiveRatingParameters_ReturnsValidResponse(int rating)
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(new Rating());

    var ratingService = new RatingService(mockRatingRepository.Object);

    Func<Rating, bool> validator = r =>
    {
      var isValid = false;

      isValid = rating switch
      {
        1 => r.Star1 == 1,
        2 => r.Star2 == 1,
        3 => r.Star3 == 1,
        4 => r.Star4 == 1,
        5 => r.Star5 == 1,
        _ => throw new Exception("Invalid rating value"),
      };

      return isValid;
    };

    // Act
    await ratingService.AddBookRatingAsync(1, rating);

    // Assert
    mockRatingRepository.Verify(
      rr => rr.SaveBookRatingAsync(It.Is<Rating>(r => validator(r))),
      Times.Once()
    );
  }

  [Fact]
  public async Task AddBookRatingAsync_InvalidRatingByBookId_ThrowsError()
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository.Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result);

    var ratingService = new RatingService(mockRatingRepository.Object);

    // Act
    var act = async () => await ratingService.AddBookRatingAsync(0, 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task AddBookRatingAsync_InvalidRatingValue_ThrowsError()
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(new Rating());

    var ratingService = new RatingService(mockRatingRepository.Object);

    // Act
    var act = async () => await ratingService.AddBookRatingAsync(0, 0);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Theory]
  [InlineData(1, 1, 1.0, 1, null, null, null, null)]
  [InlineData(2, 1, 2.0, null, 1, null, null, null)]
  [InlineData(3, 1, 3.0, null, null, 1, null, null)]
  [InlineData(4, 1, 4.0, null, null, null, 1, null)]
  [InlineData(5, 1, 5.0, null, null, null, null, 1)]
  [InlineData(1, 2, 1.0, 2, null, null, null, null)]
  [InlineData(2, 2, 2.0, null, 2, null, null, null)]
  [InlineData(3, 2, 3.0, null, null, 2, null, null)]
  [InlineData(4, 2, 4.0, null, null, null, 2, null)]
  [InlineData(5, 2, 5.0, null, null, null, null, 2)]
  [InlineData(5, 21, 3.1, 10, null, null, null, 11)]
  [InlineData(1, 6, 2.67, 2, 1, 1, 1, 1)]
  public async Task AddBookRatingAsync_RatingValues_ReturnsValidResponse(
    int initialRatingToAdd,
    int expectedStarsTotalSum,
    double expectedStarsWeightedAverage,
    int? expectedStar1,
    int? expectedStar2,
    int? expectedStar3,
    int? expectedStar4,
    int? expectedStar5
  )
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    // Stars should be null or positive values, zero is mapped to null. This is a business logic rule on the service.
    // Since this test uses InlineData, some treatment has to be done in order to properly evaluate the template.
    // This function should be run on the `initialRatingToAdd` parameter star, so that it assings the right value
    // to the mock before running the service method.
    var mockStarValue = (int? star) =>
    {
      if (star is not null)
      {
        return star - 1 == 0 ? null : star - 1;
      }
      return star;
    };

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(
        new Rating()
        {
          Star1 = initialRatingToAdd == 1 ? mockStarValue(expectedStar1) : expectedStar1,
          Star2 = initialRatingToAdd == 2 ? mockStarValue(expectedStar2) : expectedStar2,
          Star3 = initialRatingToAdd == 3 ? mockStarValue(expectedStar3) : expectedStar3,
          Star4 = initialRatingToAdd == 4 ? mockStarValue(expectedStar4) : expectedStar4,
          Star5 = initialRatingToAdd == 5 ? mockStarValue(expectedStar5) : expectedStar5,
        }
      );

    var ratingService = new RatingService(mockRatingRepository.Object);

    // Act
    await ratingService.AddBookRatingAsync(0, initialRatingToAdd);

    // Assert
    mockRatingRepository.Verify(rr =>
      rr.SaveBookRatingAsync(
        It.Is<Rating>(r =>
          r.Star1 == expectedStar1
          && r.Star2 == expectedStar2
          && r.Star3 == expectedStar3
          && r.Star4 == expectedStar4
          && r.Star5 == expectedStar5
          && r.StarsAverage == expectedStarsWeightedAverage
          && r.StarsTotal == expectedStarsTotalSum
        )
      )
    );
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public async Task DeleteBookRatingAsync_PositiveRatingParameters_ReturnsValidResponse(int rating)
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(() =>
      {
        var newRating = new Rating();
        switch (rating)
        {
          case 1:
            newRating.Star1 = 2;
            break;
          case 2:
            newRating.Star2 = 2;
            break;
          case 3:
            newRating.Star3 = 2;
            break;
          case 4:
            newRating.Star4 = 2;
            break;
          case 5:
            newRating.Star5 = 2;
            break;
          default:
            throw new Exception("Invalid rating value");
        }

        return newRating;
      });

    var ratingService = new RatingService(mockRatingRepository.Object);

    Func<Rating, bool> validator = r =>
    {
      var isValid = false;

      isValid = rating switch
      {
        1 => r.Star1 == 1,
        2 => r.Star2 == 1,
        3 => r.Star3 == 1,
        4 => r.Star4 == 1,
        5 => r.Star5 == 1,
        _ => throw new Exception("Invalid rating value"),
      };

      return isValid;
    };

    // Act
    await ratingService.DeleteBookRatingAsync(1, rating);

    // Assert
    mockRatingRepository.Verify(
      rr => rr.SaveBookRatingAsync(It.Is<Rating>(r => validator(r))),
      Times.Once()
    );
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public async Task DeleteBookRatingAsync_DefaultRatingParameters_ReturnsValidResponse(int rating)
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(new Rating());

    var ratingService = new RatingService(mockRatingRepository.Object);

    Func<Rating, bool> validator = r =>
    {
      var isValid = false;

      isValid = rating switch
      {
        1 => r.Star1 == null,
        2 => r.Star2 == null,
        3 => r.Star3 == null,
        4 => r.Star4 == null,
        5 => r.Star5 == null,
        _ => throw new Exception("Invalid rating value"),
      };

      return isValid;
    };

    // Act
    await ratingService.DeleteBookRatingAsync(1, rating);

    // Assert
    mockRatingRepository.Verify(
      rr => rr.SaveBookRatingAsync(It.Is<Rating>(r => validator(r))),
      Times.Once()
    );
  }

  [Fact]
  public async Task DeleteBookRatingAsync_InvalidRatingByBookId_ThrowsError()
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository.Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result);

    var ratingService = new RatingService(mockRatingRepository.Object);

    // Act
    var act = async () => await ratingService.DeleteBookRatingAsync(0, 1);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task DeleteBookRatingAsync_InvalidRatingValue_ThrowsError()
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(new Rating());

    var ratingService = new RatingService(mockRatingRepository.Object);

    // Act
    var act = async () => await ratingService.DeleteBookRatingAsync(0, 0);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  [Theory]
  [InlineData(1, null, null, null, null, null, null, null)]
  [InlineData(2, null, null, null, null, null, null, null)]
  [InlineData(3, null, null, null, null, null, null, null)]
  [InlineData(4, null, null, null, null, null, null, null)]
  [InlineData(5, null, null, null, null, null, null, null)]
  [InlineData(1, 1, 1.0, 1, null, null, null, null)]
  [InlineData(2, 1, 2.0, null, 1, null, null, null)]
  [InlineData(3, 1, 3.0, null, null, 1, null, null)]
  [InlineData(4, 1, 4.0, null, null, null, 1, null)]
  [InlineData(5, 1, 5.0, null, null, null, null, 1)]
  [InlineData(5, 20, 3.0, 10, null, null, null, 10)]
  [InlineData(1, 5, 3.0, 1, 1, 1, 1, 1)]
  public async Task DeleteBookRatingAsync_RatingValues_ReturnsValidResponse(
    int initialRatingToRemove,
    int? expectedStarsTotalSum,
    double? expectedStarsWeightedAverage,
    int? expectedStar1,
    int? expectedStar2,
    int? expectedStar3,
    int? expectedStar4,
    int? expectedStar5
  )
  {
    // Arrange
    var mockRatingRepository = new Mock<IRatingRepository>();

    // Since this test uses InlineData, some treatment has to be done in order to properly evaluate the template.
    // This function should be run on the `initialRatingToRemove` parameter star, so that it assings the right value
    // to the mock before running the service method.
    var mockStarValue = (int? star) =>
    {
      if (star is not null)
      {
        return star + 1;
      }
      return star;
    };

    mockRatingRepository
      .Setup(rr => rr.GetRatingByBookIdAsync(It.IsAny<int>()).Result)
      .Returns(
        new Rating()
        {
          Star1 = initialRatingToRemove == 1 ? mockStarValue(expectedStar1) : expectedStar1,
          Star2 = initialRatingToRemove == 2 ? mockStarValue(expectedStar2) : expectedStar2,
          Star3 = initialRatingToRemove == 3 ? mockStarValue(expectedStar3) : expectedStar3,
          Star4 = initialRatingToRemove == 4 ? mockStarValue(expectedStar4) : expectedStar4,
          Star5 = initialRatingToRemove == 5 ? mockStarValue(expectedStar5) : expectedStar5,
        }
      );

    var ratingService = new RatingService(mockRatingRepository.Object);

    // Act
    await ratingService.DeleteBookRatingAsync(0, initialRatingToRemove);

    // Assert
    mockRatingRepository.Verify(rr =>
      rr.SaveBookRatingAsync(
        It.Is<Rating>(r =>
          r.Star1 == expectedStar1
          && r.Star2 == expectedStar2
          && r.Star3 == expectedStar3
          && r.Star4 == expectedStar4
          && r.Star5 == expectedStar5
          && r.StarsAverage == expectedStarsWeightedAverage
          && r.StarsTotal == expectedStarsTotalSum
        )
      )
    );
  }
}
