using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class ReviewRequestEditParamsValidator : AbstractValidator<ReviewRequestEditParams>
{
  public ReviewRequestEditParamsValidator()
  {
    RuleFor(obj => obj.reviewId).NotNull().WithMessage("reviewId must not be null.");
  }
}
