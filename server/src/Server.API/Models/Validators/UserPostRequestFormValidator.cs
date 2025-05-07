using FileSignatures;
using FileSignatures.Formats;
using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class UserPostRequestFormValidator : AbstractValidator<UserPostRequestForm>
{
  public UserPostRequestFormValidator(IFileFormatInspector fileFormatInspector)
  {
    RuleFor(x => x.profileImg).NotEmpty();

    var allowedFileTypes = new List<string>() { ".png", ".jpeg" };

    RuleFor(x => x.profileImg)
      .Must(profileImg =>
        fileFormatInspector.DetermineFileFormat(profileImg!.OpenReadStream()) is Png or Jpeg
      )
      .WithMessage(
        $"profileImg must be one of the allowed types: {string.Join(", ", allowedFileTypes)}."
      )
      .When(f => f.profileImg is not null);

    // 2 MB
    var maxImageSizeInBytes = 2097152;

    RuleFor(x => x.profileImg!.Length)
      .ExclusiveBetween(0, maxImageSizeInBytes)
      .WithMessage(
        $"Image size is too large, max allowed size is: {maxImageSizeInBytes / 1024 / 1024} MB."
      )
      .When(f => f.profileImg is not null);
  }
}
