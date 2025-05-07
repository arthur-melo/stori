using Server.API.Models.Context;

namespace Server.IntegrationTests.Helpers;

[Collection("Collection")]
public abstract class BaseTests(ApiFactory webApplicationFactory) : IAsyncLifetime
{
  protected StoriContext _context = webApplicationFactory.GetContext();
  protected HttpClient _httpClient = webApplicationFactory.CreateClient();
  private Func<Task> _resetDatabase = webApplicationFactory.ResetDatabaseAsync;

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync()
  {
    _context.ChangeTracker.Clear();
    return _resetDatabase();
  }
}
