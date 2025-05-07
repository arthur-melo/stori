using AutoMapper;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class UserProfile : Profile
{
  public UserProfile()
  {
    CreateProjection<User, UserAuthorizedResponse>();
    CreateProjection<User, UserUnauthorizedResponse>();
    CreateMap<User, UserAuthorizedResponse>();
  }
}
