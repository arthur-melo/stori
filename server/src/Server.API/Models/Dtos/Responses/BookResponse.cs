namespace Server.API.Models.Dtos.Responses;

public record BookResponse(
  int id,
  string bookId,
  string title,
  string? series,
  string? description,
  string? language,
  string isbn,
  string? bookFormat,
  string? edition,
  int? pages,
  string? publishDate,
  string coverImg,
  string? publisher,
  RatingResponse? rating,
  ICollection<string> awards,
  ICollection<string> characters,
  ICollection<string> genres,
  ICollection<string> settings
);

public record RatingResponse(
  int? star1,
  int? star2,
  int? star3,
  int? star4,
  int? star5,
  double? starsAverage,
  int? starsTotal
);
