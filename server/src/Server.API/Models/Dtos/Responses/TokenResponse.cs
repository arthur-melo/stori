namespace Server.API.Models.Dtos.Responses;

public record TokenResponse(AccessToken? accessToken = null, RefreshToken? refreshToken = null);
