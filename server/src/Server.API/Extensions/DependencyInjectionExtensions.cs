using FileSignatures;
using Server.API.Repositories;
using Server.API.Repositories.Interfaces;
using Server.API.Services;
using Server.API.Services.Interfaces;

namespace Server.API.Extensions;

public static class DependencyInjectionExtensions
{
  public static void AddDependencyInjection(this IServiceCollection services)
  {
    services.AddScoped<IBookService, BookService>();
    services.AddScoped<IGenreService, GenreService>();
    services.AddScoped<ICharacterService, CharacterService>();
    services.AddScoped<ITitleService, TitleService>();
    services.AddScoped<IAwardService, AwardService>();
    services.AddScoped<ISettingService, SettingService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUserRatingService, UserRatingService>();
    services.AddScoped<IWishlistService, WishlistService>();
    services.AddScoped<IReadlistService, ReadlistService>();
    services.AddScoped<IReviewService, ReviewService>();
    services.AddScoped<IRatingService, RatingService>();
    services.AddScoped<IImageService, ImageService>();
    services.AddScoped<IEncryptionService, EncryptionService>();
    services.AddScoped<IDateTimeService, DateTimeService>();
    services.AddSingleton<IFileFormatInspector>(new FileFormatInspector());

    services.AddScoped<IBookRepository, BookRepository>();
    services.AddScoped<IGenreRepository, GenreRepository>();
    services.AddScoped<ICharacterRepository, CharacterRepository>();
    services.AddScoped<ITitleRepository, TitleRepository>();
    services.AddScoped<IAwardRepository, AwardRepository>();
    services.AddScoped<ISettingRepository, SettingRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<ITokenRepository, TokenRepository>();
    services.AddScoped<IUserRatingRepository, UserRatingRepository>();
    services.AddScoped<IWishlistRepository, WishlistRepository>();
    services.AddScoped<IReadlistRepository, ReadlistRepository>();
    services.AddScoped<IReviewRepository, ReviewRepository>();
    services.AddScoped<IRatingRepository, RatingRepository>();
    services.AddScoped<IImageRepository, ImageRepository>();
  }
}
