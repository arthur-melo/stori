using System.Net.Http.Json;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;

namespace Server.IntegrationTests.Helpers;

class Utils
{
  public static async Task<TokenResponse> CreateUserAndGetTokenAsync(
    HttpClient httpClient,
    string username,
    string email
  )
  {
    // Creates a user, log in and retrieve the login token.
    var password = "password";

    var signupRequest = new SignupRequest(username, "name", email, password);
    var signinRequest = new SigninRequest(email, password);

    // Add user to the database.
    var signupResponse = await httpClient.PostAsJsonAsync("/api/v1/auth/signup", signupRequest);

    // Logs the user in.
    var signinResponse = await httpClient.PostAsJsonAsync("/api/v1/auth/signin", signinRequest);
    var parsedSigninContent = await signinResponse.Content.ReadFromJsonAsync<TokenResponse>();

    if (parsedSigninContent is null)
    {
      throw new Exception("Error getting user token");
    }

    return parsedSigninContent;
  }
}
