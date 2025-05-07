namespace Server.API.Models.Dtos.Requests;

public record ReviewRequestUsernameParams(string? username);

public record ReviewRequestBookParams(int? bookId);

public record ReviewRequestNewCommentBody(string? text);

public record ReviewRequestCommentBody(int? reviewId);

public record ReviewRequestEditParams(int? reviewId);
