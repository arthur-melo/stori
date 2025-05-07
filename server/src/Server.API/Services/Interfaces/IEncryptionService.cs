namespace Server.API.Services.Interfaces;

public interface IEncryptionService
{
  public bool VerifyPassword(string password, string hash);
  public string HashPassword(string password);
}
