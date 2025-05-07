using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class PaginatedBookRequestValidator : AbstractValidator<PaginatedBookRequest>
{
  public PaginatedBookRequestValidator()
  {
    var allowedPageSizeValues = new HashSet<int> { 10, 25, 50, 100 };

    RuleFor(obj => obj.pageSize)
      .Must(pageSize => allowedPageSizeValues.Contains(pageSize!.Value))
      .WithMessage(
        $"pageSize must be one of the allowed values: {string.Join(", ", allowedPageSizeValues)}."
      );

    RuleFor(obj => obj.pageNumber).GreaterThan(0).WithMessage("pageNumber must be greater than 0.");

    RuleFor(x => x.bookId).NotNull().WithMessage("bookId must not be null.");
  }
}
