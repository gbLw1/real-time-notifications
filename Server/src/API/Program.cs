using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RTN.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new() { Title = "RTN.API", Version = "v1" });

    o.AddSecurityDefinition("GUID", new OpenApiSecurityScheme
    {
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


// // config httpclient
// builder.Services.AddHttpClient<HttpClientOptions, HttpClient>(o => o.BaseAddress = new Uri("https://localhost:5001"))
//     .AddPolicyHandlerFromRegistry((services, key) => services.GetRequiredService<IAsyncPolicy<HttpResponseMessage>>(key));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RTN.API v1"));
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

record HttpClientOptions(string BaseAddress, string Accept, string UserAgent);
