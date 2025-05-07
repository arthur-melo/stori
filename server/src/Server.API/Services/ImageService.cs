using Microsoft.Extensions.Options;
using Server.API.Options;
using Server.API.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Server.API.Services;

public class ImageService(
  IOptions<FileUploadOptions> fileUploadOptions,
  IImageRepository imageRepository
) : IImageService
{
  private readonly FileUploadOptions _fileUploadOptions = fileUploadOptions.Value;
  private readonly IImageRepository _imageRepository = imageRepository;

  public async Task<string> ProcessImageAsync(IFormFile file)
  {
    // Image processing
    var image = Image.Load(file.OpenReadStream());

    // Resize to 400x400
    image.Mutate(x => x.Resize(new ResizeOptions() { Size = new Size(400, 400) }));

    // Save new image to static files dir
    var fileName = $"{Guid.NewGuid()}.jpg";
    var filePath = Path.Combine(_fileUploadOptions.Path, fileName);

    if (!Directory.Exists(_fileUploadOptions.Path))
    {
      _imageRepository.CreateDirectory(_fileUploadOptions.Path);
    }

    await _imageRepository.AddImageAsync(image, filePath);

    return fileName;
  }

  public void DeleteImage(string fileName)
  {
    var path = Path.Combine(_fileUploadOptions.Path, fileName);
    _imageRepository.DeleteImage(path);
  }

  public bool ValidateDirectory(string path)
  {
    return _imageRepository.IsValidDirectory(path);
  }
}
