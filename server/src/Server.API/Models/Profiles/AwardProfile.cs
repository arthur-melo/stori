using AutoMapper;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class AwardProfile : Profile
{
  public AwardProfile()
  {
    CreateProjection<Award, string?>().ConvertUsing(award => award.Name);
  }
}
