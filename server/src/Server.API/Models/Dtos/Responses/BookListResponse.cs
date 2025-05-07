namespace Server.API.Models.Dtos.Responses;

public record BookListResponse(
  int id,
  string bookId,
  string title,
  string? description,
  string? publishDate,
  string coverImg,
  BookListRatingResponse? rating
);

public record BookListRatingResponse(double starsAverage, int starsTotal);
