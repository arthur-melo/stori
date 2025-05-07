using AutoMapper;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class BookProfile : Profile
{
  public BookProfile()
  {
    CreateProjection<Book, string?>().ConvertUsing(book => book.Title);

    CreateProjection<Rating, RatingResponse>();
    CreateProjection<Rating, BookListRatingResponse>();

    CreateProjection<Book, BookListResponse>()
      .ForCtorParam(
        "publishDate",
        opt =>
          opt.MapFrom(src =>
            src.PublishDate == null ? null : src.PublishDate!.Value.ToString("yyyy-MM-dd")
          )
      )
      .ForCtorParam(
        "rating",
        opt => opt.MapFrom(src => src.Rating!.StarsTotal == null ? null : src.Rating)
      );

    CreateProjection<Book, BookResponse>()
      .ForMember(
        b => b.publishDate,
        opt => opt.MapFrom(src => src.PublishDate!.Value.ToString("yyyy-MM-dd"))
      )
      .ForCtorParam(
        "rating",
        opt => opt.MapFrom(src => src.Rating!.StarsTotal == null ? null : src.Rating)
      )
      .ForCtorParam("publisher", opt => opt.MapFrom(src => src.Publisher!.Name))
      .ForCtorParam(
        "awards",
        opt => opt.MapFrom(src => src.Awards.Select(award => award.Name).ToList())
      )
      .ForCtorParam(
        "characters",
        opt => opt.MapFrom(src => src.Characters.Select(character => character.Name).ToList())
      )
      .ForCtorParam(
        "genres",
        opt => opt.MapFrom(src => src.Genres.Select(genre => genre.Name).ToList())
      )
      .ForCtorParam(
        "settings",
        opt => opt.MapFrom(src => src.Settings.Select(setting => setting.Name).ToList())
      );
  }
}
