using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class SignupRequestValidator : AbstractValidator<SignupRequest>
{
  public SignupRequestValidator()
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

    RuleFor(obj => obj.name).NotEmpty().MaximumLength(64);

    RuleFor(obj => obj.email).NotEmpty().EmailAddress().MaximumLength(256);

    RuleFor(obj => obj.password).NotEmpty().MinimumLength(4).MaximumLength(256);
  }
}
