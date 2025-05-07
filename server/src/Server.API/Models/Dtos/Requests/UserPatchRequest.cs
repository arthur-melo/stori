namespace Server.API.Models.Dtos.Requests;

public record UserPatchRequestBody(string? email, string? password, string? username, string? name);

public record UserPatchRequestParameters(string? username);
