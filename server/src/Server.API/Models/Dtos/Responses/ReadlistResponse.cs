namespace Server.API.Models.Dtos.Responses;

public record ReadlistResponse(BookListResponse book, DateTime createdAt);

public record ReadlistByBookResponse(BookListResponse book, DateTime createdAt);
