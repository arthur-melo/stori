using Server.API.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsExtensions
{
  public static void AddCustomOptions(this IServiceCollection services, IConfiguration config)
  {
    services.Configure<JWTOptions>(config.GetSection(nameof(JWTOptions)));
    services.Configure<FileUploadOptions>(config.GetSection(nameof(FileUploadOptions)));
    services.Configure<ConstantsOptions>(config.GetSection(nameof(ConstantsOptions)));
    services.Configure<StoriDatabaseOptions>(config.GetSection(nameof(StoriDatabaseOptions)));
  }
}
