namespace Server.API.Models.Dtos.Requests;

public record PaginatedListRequest(int? pageSize = 10, int? pageNumber = 1, string? name = null);
