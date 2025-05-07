namespace Server.API.Services;

using BCrypt.Net;
using Server.API.Services.Interfaces;

public class EncryptionService() : IEncryptionService
{
  public bool VerifyPassword(string password, string hash)
  {
    return BCrypt.Verify(password, hash);
  }

  public string HashPassword(string password)
  {
    return BCrypt.HashPassword(password);
  }
}
