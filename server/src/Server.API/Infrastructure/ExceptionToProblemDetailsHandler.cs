using Microsoft.AspNetCore.Diagnostics;
using Server.API.Exceptions;

namespace Server.API.Infrastructure;

public class ExceptionToProblemDetailsHandler(
  IProblemDetailsService problemDetailsService,
  ILogger<ExceptionToProblemDetailsHandler> logger
) : IExceptionHandler
{
  private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;
  private readonly ILogger<ExceptionToProblemDetailsHandler> _logger = logger;

  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken
  )
  {
    _logger.LogError(exception.Message, exception);

    httpContext.Response.StatusCode = exception switch
    {
      NotFoundException => StatusCodes.Status404NotFound,
      ValidationException => StatusCodes.Status400BadRequest,
      UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
      _ => StatusCodes.Status500InternalServerError,
    };

    return await _problemDetailsService.TryWriteAsync(
      new ProblemDetailsContext
      {
        HttpContext = httpContext,
        ProblemDetails =
        {
          Detail =
            httpContext.Response.StatusCode == 500 ? "Internal server error" : exception.Message,
          Type =
            httpContext.Response.StatusCode == 500
              ? "InternalServerErrorException"
              : exception.GetType().Name,
        },
        Exception = exception,
      }
    );
  }
}
