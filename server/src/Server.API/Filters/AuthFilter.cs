using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Server.API.Filters;

public class AuthFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    var hasAuthorize = context
      .MethodInfo.GetCustomAttributes(true)
      .OfType<AuthorizeAttribute>()
      .Any();

    if (!hasAuthorize)
    {
      return;
    }

    operation.Security.Add(
      new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme },
          },
          new List<string>()
        },
      }
    );
  }
}
