namespace Server.API.Models.Dtos.Requests;

public record UserRequest(string username);

public record PaginatedUserRequest(string username, int? pageSize = 10, int? pageNumber = 1);
