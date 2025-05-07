using Server.API.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ExceptionHandlerExtensions
{
  public static void AddCustomExceptionHandler(this IServiceCollection services)
  {
    services.AddProblemDetails(options =>
      options.CustomizeProblemDetails = ctx =>
      {
        ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
        ctx.ProblemDetails.Extensions.Add(
          "instance",
          $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}"
        );
      }
    );

    services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();
  }
}
