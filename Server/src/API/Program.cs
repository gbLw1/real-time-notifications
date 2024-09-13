using RTN.API.Shared.Extensions;

WebApplication
    .CreateBuilder(args)
    .AddCustomApplication()
    .Build()
    .UseCustomApplication()
    .Run();
