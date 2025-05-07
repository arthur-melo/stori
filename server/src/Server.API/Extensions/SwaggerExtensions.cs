using System.Reflection;
using Microsoft.OpenApi.Models;
using Server.API.Filters;

namespace Server.API.Extensions;

public static class SwaggerExtensions
{
  public static void AddCustomSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(cfg =>
    {
      cfg.UseAllOfToExtendReferenceSchemas();
      cfg.SupportNonNullableReferenceTypes();
      cfg.NonNullableReferenceTypesAsRequired();

      cfg.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
          Title = "Stori API",
          Version = "v1",
          Description = "Stori Backend API",
        }
      );

      cfg.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description =
            "JSON Web Token to access resources. Paste your token as: Bearer {access_token}",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer",
        }
      );

      cfg.OperationFilter<AuthFilter>();

      var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      cfg.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
  }

  public static void UseCustomSwagger(this IApplicationBuilder app)
  {
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
      options.SwaggerEndpoint("/swagger/v1/swagger.json", "Stori API");
      options.DocumentTitle = "Stori API";
      options.RoutePrefix = string.Empty;
    });
  }
}
