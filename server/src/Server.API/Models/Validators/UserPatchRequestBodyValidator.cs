using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class UserPatchRequestBodyValidator : AbstractValidator<UserPatchRequestBody>
{
  public UserPatchRequestBodyValidator()
  {
    RuleFor(user => user.username)
      .Matches(@"^[a-z0-9]+([-_][a-z0-9]+)*$")
      .WithMessage(
        "Invalid characters in username. Allowed characters are lowercase, numbers, dashes/underlines"
      )
      .Length(3, 32)
      .WithMessage("Username must be between 3 and 32 characters long")
      .When(user => !string.IsNullOrEmpty(user.username));

    RuleFor(user => user.name)
      .NotEmpty()
      .MaximumLength(64)
      .When(user => !string.IsNullOrEmpty(user.name));

    RuleFor(user => user.email)
      .NotEmpty()
      .EmailAddress()
      .MaximumLength(256)
      .When(user => !string.IsNullOrEmpty(user.email));

    RuleFor(user => user.password)
      .NotEmpty()
      .MinimumLength(4)
      .MaximumLength(256)
      .When(user => !string.IsNullOrEmpty(user.password));
  }
}
