using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using RTN.API.Data;
using RTN.API.Services;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace RTN.API.Shared.Extensions;

public static class WebApplicationExtensions {
    public static WebApplicationBuilder AddCustomApplication(
        this WebApplicationBuilder builder,
        Action<SwaggerGenOptions>? swaggerOptions = null) {
        // AppSettings
        var appSettings = builder.Configuration.GetSection("AppSettings")
            ?? throw new ArgumentNullException("AppSettings is required in appsettings.json or environment variables.");
        builder.Services.Configure<AppSettings>(appSettings);

        // Cryptography
        builder.Services.AddSingleton<CryptoService>();

        // Authentication
        builder.Services.AddAuthentication(options => options.AddScheme<DefaultAuthenticationHandler>("Bearer", "Bearer"));

        // JSON options
        builder.Services.AddControllers()
            .AddJsonOptions(o => {
                o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
#if DEBUG
                o.JsonSerializerOptions.WriteIndented = true;
#endif
            });

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o => {
            o.SwaggerDoc("v1", new() { Title = "RTN.API", Version = "v1" });

            // o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
            //     Name = "Authorization",
            //     Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //     Type = SecuritySchemeType.Http,
            //     BearerFormat = "Bearer {token}",
            //     In = ParameterLocation.Header,
            //     Scheme = "Bearer",
            // });

            o.AddSecurityDefinition("GUID", new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Description = "Please enter your GUID token in the field. Example: '12345678-1234-1234-1234-123456789012'",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "GUID"
            });

            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "GUID"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Database config
        builder.Services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // Notification API (WebSocket service)
        var notificationApiBaseUrl = builder.Configuration["NotificationApiSettings:BaseUrl"]
            ?? throw new ArgumentNullException("NotificationApi:BaseUrl is required in appsettings.json or environment variables.");
        builder.Services.AddHttpClient<NotificationService>(client => {
            client.BaseAddress = new Uri(notificationApiBaseUrl);
        });

        return builder;
    }

    public static WebApplication UseCustomApplication(this WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RTN.API v1"));
            app.UseCors(options => {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
        }

        // use middleware for exception handling
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
