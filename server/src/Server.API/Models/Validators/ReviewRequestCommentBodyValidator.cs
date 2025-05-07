using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class ReviewRequestCommentBodyValidator : AbstractValidator<ReviewRequestCommentBody>
{
  public ReviewRequestCommentBodyValidator()
  {
    RuleFor(obj => obj.reviewId).NotNull();
  }
}
