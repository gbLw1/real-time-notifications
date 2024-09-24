using Microsoft.OpenApi.Models;

namespace RTN.API.Shared.Configuration;

public static class SwaggerConfiguration {
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services) {
        services.AddSwaggerGen(o => {
            o.SwaggerDoc("v1", new() { Title = "RTN.API", Version = "v1" });

            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Name = "Authorization",
                Description = "Authorization header using the Bearer scheme. Example: \"Bearer {token}\" or just the token",
                Type = SecuritySchemeType.Http,
                BearerFormat = "{token}",
                In = ParameterLocation.Header,
                Scheme = "Bearer",
            });

            // Add the AuthenticationRequirementsOperationFilter to the Swagger pipeline to add the lock icon to the endpoints
            o.OperationFilter<AuthenticationRequirementsOperationFilter>();
        });

        return services;
    }
}
