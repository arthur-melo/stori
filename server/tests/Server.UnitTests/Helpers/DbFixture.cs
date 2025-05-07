using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Server.API.Models.Context;
using Server.API.Options;

namespace Server.UnitTests.Helpers;

public class DbFixture : IDisposable
{
  private StoriContext _context = default!;

  public StoriContext GetContext() => _context;

  public DbFixture()
  {
    var options = new DbContextOptionsBuilder<StoriContext>()
      .UseSqlite("Filename=:memory:")
      .Options;

    _context = new StoriContext(options, new Mock<IOptions<StoriDatabaseOptions>>().Object);
    _context.Database.OpenConnection();
    _context.Database.EnsureCreated();
  }

  public void Dispose()
  {
    _context.Database.CloseConnection();
    _context.Dispose();
  }
}
