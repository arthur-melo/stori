using AutoMapper;

namespace Server.UnitTests.Helpers;

public class AutoMapperFactory
{
  public IMapper mapper;
  public MapperConfiguration config;

  public AutoMapperFactory()
  {
    config = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

    mapper = config.CreateMapper();
  }
}
