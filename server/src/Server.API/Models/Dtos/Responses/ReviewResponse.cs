namespace Server.API.Models.Dtos.Responses;

public record ReviewResponse(
  int id,
  BookListResponse book,
  string text,
  DateTime createdAt,
  int? rating
);

public record ReviewBookResponse(
  int id,
  ReviewBookUserResponse author,
  string text,
  DateTime createdAt,
  int? rating
);

public record ReviewBookUserResponse(string username, string name, string? profileImg);
