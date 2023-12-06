using Application;
using Carter;
using Infrastructure;
using Persistence;
using Presentation;
using Serilog;
using Web.API;
using Web.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddHttpContextAccessor()
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration)
    .AddPresentation(builder.Configuration)
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

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler(opt => { });

app.MapCarter();

app.Run();

public partial class Program { }