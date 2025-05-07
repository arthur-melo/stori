using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class ReviewRequestUsernameParamsValidator : AbstractValidator<ReviewRequestUsernameParams>
{
  public ReviewRequestUsernameParamsValidator()
  {
    RuleFor(x => x.username)
      .NotEmpty()
      .Matches(@"^[a-z0-9]+([-_][a-z0-9]+)*$")
      .WithMessage(
        "Invalid characters in username. Allowed characters are lowercase, numbers, dashes/underlines"
      )
      .Length(3, 32)
      .WithMessage("Username must be between 3 and 32 characters long");
  }
}
