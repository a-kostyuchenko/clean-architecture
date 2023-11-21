using Application;
using Carter;
using Infrastructure;
using Persistence;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration)
    .AddPresentation();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapCarter();

app.Run();