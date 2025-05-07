using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Server.API.Models.Dtos.Requests;
using Server.API.Models.Dtos.Responses;
using Server.API.Services.Interfaces;

namespace Server.API.Apis;

public static class BookApi
{
  public static RouteGroupBuilder MapBooksApi(this RouteGroupBuilder app)
  {
    app.MapGet("/books", GetBooksAsync);
    app.MapGet("/books/{id}", GetBookByIdAsync);
    app.MapGet("/books/bookId/{bookId}", GetBookByBookIdAsync);

    return app;
  }

  /// <summary>
  /// Returns a list of books
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/books?pageSize=10&amp;pageNumber=1&amp;orderBy=rating&amp;genre=genreFilter&amp;title=titleFilter&amp;character=characterFilter&amp;award=awardFilter&amp;setting=settingFilter
  ///
  /// All query string parameters are optional
  ///
  /// Valid `pageSize` values are: 10, 25, 50, 100
  ///
  /// Valid `orderBy` values: rating or date
  /// </remarks>
  /// <param name="request">Book data</param>
  /// <response code="200">Returns a paginated list of books</response>
  /// <response code="400">If the parameters validation failed</response>
  [ProducesResponseType(
    StatusCodes.Status200OK,
    Type = typeof(PaginatedListEnvelope<BookListResponse>)
  )]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public static async Task<
    Results<Ok<PaginatedListEnvelope<BookListResponse>>, ValidationProblem>
  > GetBooksAsync([FromServices] IBookService bookService, [AsParameters] BookListRequest request)
  {
    var books = await bookService.GetBooksAsync(
      request.pageSize!.Value,
      request.pageNumber!.Value,
      request.orderBy!,
      request.genre,
      request.title,
      request.character,
      request.award,
      request.setting
    );

    return TypedResults.Ok(books);
  }

  /// <summary>
  /// Returns a single book by id
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/books/{id}
  ///
  /// </remarks>
  /// <param name="id">Book id</param>
  /// <response code="200">Returns a single book data in detailed format</response>
  /// <response code="404">If the given `id` was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<BookResponse>))]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<Results<Ok<Envelope<BookResponse>>, NotFound>> GetBookByIdAsync(
    [FromServices] IBookService bookService,
    int id
  )
  {
    var book = await bookService.GetBookByIdAsync(id);

    return TypedResults.Ok(book);
  }

  /// <summary>
  /// Returns a single book by book id (String)
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///
  ///     GET /api/v1/books/{bookId}
  ///
  /// </remarks>
  /// <param name="bookId">Book id</param>
  /// <response code="200">Returns a single book data in detailed format</response>
  /// <response code="404">If the given `id` was not found</response>
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<BookResponse>))]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public static async Task<Results<Ok<Envelope<BookResponse>>, NotFound>> GetBookByBookIdAsync(
    [FromServices] IBookService bookService,
    string bookId
  )
  {
    var book = await bookService.GetBookByBookIdAsync(bookId);

    return TypedResults.Ok(book);
  }
}
