using Server.API.Models.Context;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseExtensions
{
  public static void AddDatabase(this IServiceCollection services)
  {
    services.AddDbContext<StoriContext>();
  }
}
