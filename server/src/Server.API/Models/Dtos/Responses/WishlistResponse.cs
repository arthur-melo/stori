namespace Server.API.Models.Dtos.Responses;

public record WishlistResponse(BookListResponse book, DateTime createdAt);

public record WishlistByBookResponse(BookListResponse book, DateTime createdAt);
