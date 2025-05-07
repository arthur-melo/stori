using Microsoft.EntityFrameworkCore;
using Server.API.Models.Context;

namespace Web.API.Extensions;

public static class MigrationExtensions
{
  public static void ApplyMigrations(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<StoriContext>();

    dbContext.Database.Migrate();
  }
}
