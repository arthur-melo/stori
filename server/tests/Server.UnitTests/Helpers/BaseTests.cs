using Server.API.Models.Context;

namespace Server.UnitTests.Helpers;

public abstract class BaseTests()
{
  protected StoriContext _context = new DbFixture().GetContext();
}
