using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class ReadlistRequestParametersValidator : AbstractValidator<ReadlistRequestParams>
{
  public ReadlistRequestParametersValidator()
  {
    RuleFor(x => x.username)
      .NotNull()
      .NotEmpty()
      .Matches(@"^[a-z0-9]+([-_][a-z0-9]+)*$")
      .WithMessage(
        "Invalid characters in username. Allowed characters are lowercase, numbers, dashes/underlines"
      )
      .Length(3, 32)
      .WithMessage("Username must be between 3 and 32 characters long");

    RuleFor(x => x.bookId).NotNull();
  }
}
