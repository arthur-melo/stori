using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class ReviewRequestBookParamsValidator : AbstractValidator<ReviewRequestBookParams>
{
  public ReviewRequestBookParamsValidator()
  {
    RuleFor(obj => obj.bookId).NotNull().WithMessage("bookId must not be null.");
  }
}
