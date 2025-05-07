using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class UserRatingRequestBodyValidator : AbstractValidator<UserRatingRequestBody>
{
  public UserRatingRequestBodyValidator()
  {
    RuleFor(obj => obj.rating).NotNull().GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
  }
}
