using Application;
using Carter;
using Infrastructure;
using Persistence;
using Presentation;
using Serilog;
using WebAPI;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration)
    .AddPresentation()
    .AddAuthenticationAndAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseExceptionHandler(opt => { });

app.MapCarter();

app.Run();