using AutoMapper;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class GenreProfile : Profile
{
  public GenreProfile()
  {
    CreateProjection<Genre, string?>().ConvertUsing(genre => genre.Name);
  }
}
