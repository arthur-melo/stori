using AutoMapper;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class ReadlistProfile : Profile
{
  public ReadlistProfile()
  {
    CreateProjection<Readlist, ReadlistResponse>()
      .ForCtorParam("book", opt => opt.MapFrom(src => src.Book));

    CreateProjection<Readlist, ReadlistByBookResponse>()
      .ForCtorParam("book", opt => opt.MapFrom(src => src.Book));
  }
}
