using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class SigninRequestValidator : AbstractValidator<SigninRequest>
{
  public SigninRequestValidator()
  {
    RuleFor(obj => obj.email).NotEmpty().EmailAddress().MaximumLength(256);

    RuleFor(obj => obj.password).NotEmpty().MinimumLength(4).MaximumLength(256);
  }
}
