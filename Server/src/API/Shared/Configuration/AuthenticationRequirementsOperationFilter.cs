using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace RTN.API.Shared.Configuration;

public class AuthenticationRequirementsOperationFilter : IOperationFilter {
    // This method is basically a middleware for Swagger that adds the lock icon to the endpoints
    // If the endpoint has the [Authorize] attribute, we add the Bearer token requirement
    public void Apply(OpenApiOperation operation, OperationFilterContext context) {
        if (!context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()) {
            return;
        }

        operation.Security ??= [];

        var scheme = new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        operation.Security.Add(new OpenApiSecurityRequirement {
            [scheme] = []
        });
    }

}
