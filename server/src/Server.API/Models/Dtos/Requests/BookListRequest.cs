namespace Server.API.Models.Dtos.Requests;

public record BookListRequest(
  int? pageSize = 10,
  int? pageNumber = 1,
  string? genre = null,
  string? title = null,
  string? character = null,
  string? award = null,
  string? setting = null,
  string? orderBy = "rating"
);
