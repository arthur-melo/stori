using AutoMapper;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class WishlistProfile : Profile
{
  public WishlistProfile()
  {
    CreateProjection<Wishlist, WishlistResponse>()
      .ForCtorParam("book", opt => opt.MapFrom(src => src.Book));

    CreateProjection<Wishlist, WishlistByBookResponse>()
      .ForCtorParam("book", opt => opt.MapFrom(src => src.Book));
  }
}
