using AutoMapper;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class CharacterProfile : Profile
{
  public CharacterProfile()
  {
    CreateProjection<Character, string?>().ConvertUsing(character => character.Name);
  }
}
