namespace Server.API.Models.Dtos.Responses;

public record UserAuthorizedResponse(
  string username,
  string email,
  string name,
  string? profileImg,
  DateTime createdAt
);

public record UserUnauthorizedResponse(
  string username,
  string name,
  string? profileImg,
  DateTime createdAt
);
