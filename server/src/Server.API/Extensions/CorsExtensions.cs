namespace Microsoft.Extensions.DependencyInjection;

public static class CorsExtensions
{
  public static void AddCustomCors(this IServiceCollection services)
  {
    services.AddCors(options =>
    {
      options.AddDefaultPolicy(policy =>
      {
        policy.WithOrigins("*");
      });
    });
  }
}
