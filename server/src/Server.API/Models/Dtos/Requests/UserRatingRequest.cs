namespace Server.API.Models.Dtos.Requests;

public record UserRatingRequestParams(string? username, int? bookId);

public record UserRatingRequestBody(int? rating);
