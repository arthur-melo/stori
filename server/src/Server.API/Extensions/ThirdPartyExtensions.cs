using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ThirdPartyExtensions
{
  public static void AddThirdParty(this IServiceCollection services, IHostEnvironment env)
  {
    // Automapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // FluentValidation
    services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
  }
}
