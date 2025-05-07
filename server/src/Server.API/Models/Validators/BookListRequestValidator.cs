using FluentValidation;
using Server.API.Models.Dtos.Requests;

namespace Server.API.Models.Validators;

public class BookListRequestValidator : AbstractValidator<BookListRequest>
{
  public BookListRequestValidator()
  {
    var allowedPageSizeValues = new HashSet<int> { 10, 25, 50, 100 };

    RuleFor(obj => obj.pageSize)
      .Must(pageSize => allowedPageSizeValues.Contains(pageSize!.Value))
      .WithMessage(
        $"pageSize must be one of the allowed values: {string.Join(", ", allowedPageSizeValues)}."
      );

    RuleFor(obj => obj.pageNumber).GreaterThan(0).WithMessage("pageNumber must be greater than 0.");

    var allowedOrderByValues = new List<string>() { "rating", "date" };

    RuleFor(obj => obj.orderBy)
      .Must(orderBy => allowedOrderByValues.Contains(orderBy!))
      .WithMessage(
        $"orderBy must be one of the allowed values: {string.Join(", ", allowedOrderByValues)}."
      );
  }
}
