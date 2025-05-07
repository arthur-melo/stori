using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Server.API.Models.Context;
using Testcontainers.MsSql;

namespace Server.IntegrationTests.Helpers;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private StoriContext _context = default!;

  private MsSqlContainer _mssqlDb = new MsSqlBuilder()
    .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
    .Build();

  private DbConnection _dbConnection = default!;
  private Respawner _respawner = default!;

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Test");

    builder.ConfigureServices(
      (context, services) =>
      {
        var dbContextDescriptor = services.SingleOrDefault(d =>
          d.ServiceType == typeof(DbContextOptions<StoriContext>)
        );

        if (dbContextDescriptor is not null)
        {
          services.Remove(dbContextDescriptor);
        }

        var dbConnectionDescriptor = services.SingleOrDefault(d =>
          d.ServiceType == typeof(DbConnection)
        );

        if (dbConnectionDescriptor is not null)
        {
          services.Remove(dbConnectionDescriptor);
        }

        services.AddSqlServer<StoriContext>(_mssqlDb.GetConnectionString());
      }
    );
  }

  public StoriContext GetContext()
  {
    return _context;
  }

  public async Task InitializeAsync()
  {
    await _mssqlDb.StartAsync();
    _context = Services.CreateScope().ServiceProvider.GetRequiredService<StoriContext>();
    await _context.Database.EnsureCreatedAsync();
    await InitializeDbRespawner();
  }

  public new async Task DisposeAsync()
  {
    await _mssqlDb.DisposeAsync();
  }

  public async Task ResetDatabaseAsync()
  {
    await _respawner.ResetAsync(_dbConnection);
  }

  private async Task InitializeDbRespawner()
  {
    _dbConnection = new SqlConnection(_mssqlDb.GetConnectionString());
    await _dbConnection.OpenAsync();
    _respawner = await Respawner.CreateAsync(
      _dbConnection,
      new RespawnerOptions
      {
        DbAdapter = DbAdapter.SqlServer,
        SchemasToInclude = ["dbo"],
        WithReseed = true,
      }
    );
  }
}
