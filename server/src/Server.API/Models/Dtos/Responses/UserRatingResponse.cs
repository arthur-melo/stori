namespace Server.API.Models.Dtos.Responses;

public record UserRatingResponse(BookListResponse book, int rating, DateTime createdAt);

public record UserRatingByBookResponse(int rating);
