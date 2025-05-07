using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class RefreshTokenValidatorRequest : AbstractValidator<RefreshTokenRequest>
{
  public RefreshTokenValidatorRequest()
  {
    RuleFor(obj => obj.token).NotEmpty();
  }
}
