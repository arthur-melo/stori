using AutoMapper;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class UserRatingProfile : Profile
{
  public UserRatingProfile()
  {
    CreateProjection<UserRating, UserRatingResponse>()
      .ForCtorParam("book", opt => opt.MapFrom(src => src.Book));

    CreateProjection<UserRating, UserRatingByBookResponse>();
  }
}
