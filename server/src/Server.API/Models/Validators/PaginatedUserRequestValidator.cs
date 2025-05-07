using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class PaginatedUserRequestValidator : AbstractValidator<PaginatedUserRequest>
{
  public PaginatedUserRequestValidator()
  {
    var allowedPageSizeValues = new HashSet<int> { 10, 25, 50, 100 };

    RuleFor(obj => obj.pageSize)
      .Must(pageSize => allowedPageSizeValues.Contains(pageSize!.Value))
      .WithMessage(
        $"pageSize must be one of the allowed values: {string.Join(", ", allowedPageSizeValues)}."
      );

    RuleFor(obj => obj.pageNumber).GreaterThan(0).WithMessage("pageNumber must be greater than 0.");

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
