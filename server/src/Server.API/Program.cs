using Server.API.Extensions;
using Web.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomExceptionHandler();
builder.Services.AddCustomOptions(builder.Configuration);
builder.Services.AddDependencyInjection();
builder.Services.AddDatabase();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomCors();

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddThirdParty(builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseCustomSwagger();
  app.ApplyMigrations();
}

app.UseStatusCodePages();
app.UseExceptionHandler();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseCustomRoutes(builder.Configuration);

// Hosting user profile images at: /images/uuid.png
app.UseStaticFiles();

app.Run();

// Needed for using minimal api on integration tests.
public partial class Program { }
