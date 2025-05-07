using Server.API.Services.Interfaces;

namespace Server.API.Services;

public class DateTimeService : IDateTimeService
{
  // Used for handling mocks/stubs in tests
  public DateTime Now()
  {
    return DateTime.Now;
  }
}
