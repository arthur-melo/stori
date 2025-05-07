using AutoMapper;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class ReviewProfile : Profile
{
  public ReviewProfile()
  {
    CreateMap<User, ReviewBookUserResponse>();

    CreateProjection<Review, ReviewResponse>()
      .ForCtorParam("book", opt => opt.MapFrom(src => src.Book))
      .ForCtorParam(
        "rating",
        opt =>
          opt.MapFrom(src =>
            src.User.UserRatings.Where(ur => ur.BookId == src.BookId)
              .Select(item => (int?)item.Rating)
              .FirstOrDefault()
          )
      );

    CreateMap<Review, ReviewBookResponse>()
      .ForCtorParam("author", opt => opt.MapFrom(src => src.User))
      .ForCtorParam(
        "rating",
        opt =>
          opt.MapFrom(src =>
            src.User.UserRatings.Where(ur => ur.BookId == src.BookId)
              .Select(item => (int?)item.Rating)
              .FirstOrDefault()
          )
      );
  }
}
