namespace Server.API.Models;

public class PaginatedListEnvelope<T> : Envelope<T>
{
  public int PageNumber { get; }
  public int TotalPages { get; }
  public int TotalItems { get; }
  public bool HasPreviousPage => PageNumber > 1;
  public bool HasNextPage => PageNumber < TotalPages;

  public PaginatedListEnvelope(List<T> data, int pageNumber, int totalPages, int totalItems)
    : base(data)
  {
    PageNumber = pageNumber;
    TotalPages = totalPages;
    TotalItems = totalItems;
  }
}
