using SixLabors.ImageSharp;

namespace Server.API.Services.Interfaces;

public interface IImageRepository
{
  public Task AddImageAsync(Image image, string path);
  public void DeleteImage(string fileName);
  public void CreateDirectory(string path);
  public bool IsValidDirectory(string path);
}
