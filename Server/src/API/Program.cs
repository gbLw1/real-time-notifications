using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using RTN.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => {
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
#if DEBUG
        o.JsonSerializerOptions.WriteIndented = true;
#endif
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => {
    o.SwaggerDoc("v1", new() { Title = "RTN.API", Version = "v1" });

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

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline
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

app.Run();
