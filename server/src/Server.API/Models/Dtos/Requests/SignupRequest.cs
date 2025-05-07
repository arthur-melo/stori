namespace Server.API.Models.Dtos.Requests;

public record SignupRequest(string? username, string? name, string? email, string? password);
