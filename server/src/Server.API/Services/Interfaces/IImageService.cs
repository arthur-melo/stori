namespace Server.API.Services.Interfaces;

public interface IImageService
{
  public Task<string> ProcessImageAsync(IFormFile file);

  public void DeleteImage(string fileName);

  public bool ValidateDirectory(string path);
}
