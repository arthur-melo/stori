using Server.API.Services.Interfaces;
using SixLabors.ImageSharp;

namespace Server.API.Repositories;

public class ImageRepository : IImageRepository
{
  public async Task AddImageAsync(Image image, string path)
  {
    using (var stream = File.Create(path))
    {
      await image.SaveAsJpegAsync(stream);
    }
  }

  public void DeleteImage(string fileName)
  {
    File.Delete(fileName);
  }

  public void CreateDirectory(string path)
  {
    Directory.CreateDirectory(path);
  }

  public bool IsValidDirectory(string path)
  {
    return Directory.Exists(path);
  }
}
