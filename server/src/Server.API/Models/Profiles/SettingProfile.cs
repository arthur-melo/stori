using AutoMapper;
using Server.API.Models.Entities;

namespace Server.API.Models.Profiles;

public class SettingProfile : Profile
{
  public SettingProfile()
  {
    CreateProjection<Setting, string?>().ConvertUsing(setting => setting.Name);
    ;
  }
}
