using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class ReviewRequestNewCommentBodyValidator : AbstractValidator<ReviewRequestNewCommentBody>
{
  public ReviewRequestNewCommentBodyValidator()
  {
    RuleFor(obj => obj.text).NotEmpty().MaximumLength(1024);
  }
}
