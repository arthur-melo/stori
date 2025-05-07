namespace Server.API.Models.Dtos.Requests;

public record PaginatedBookRequest(int? bookId, int? pageSize = 10, int? pageNumber = 1);
