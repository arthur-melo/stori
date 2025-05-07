using Microsoft.Extensions.Options;
using Server.API.Apis;
using Server.API.Options;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class RouteExtensions
{
  public static void UseCustomRoutes(this WebApplication app, IConfiguration config)
  {
    var routePrefix = app
      .Services.GetRequiredService<IOptions<ConstantsOptions>>()
      .Value.RoutePrefix;

    if (routePrefix is null)
    {
      throw new ArgumentNullException(nameof(routePrefix));
    }

    Func<string, RouteGroupBuilder> routeMap = tags =>
      app.MapGroup(routePrefix).WithTags(tags).AddFluentValidationAutoValidation();

    routeMap("Books").MapBooksApi();
    routeMap("Genres").MapGenresApi();
    routeMap("Characters").MapCharactersApi();
    routeMap("Titles").MapTitlesApi();
    routeMap("Awards").MapAwardsApi();
    routeMap("Settings").MapSettingsApi();
    routeMap("Auth").MapAuthApi();
    routeMap("User").MapUserApi();
    routeMap("User Rating").MapUserRatingApi();
    routeMap("Wishlist").MapWishlistApi();
    routeMap("Readlist").MapReadlistApi();
    routeMap("Review").MapReviewApi();
  }
}
