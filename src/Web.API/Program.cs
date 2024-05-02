using System.Reflection;
using System.Threading.RateLimiting;
using Application;
using Infrastructure;
using Persistence;
using Serilog;
using Web.API;
using Web.API.Extensions;
using Web.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddHttpContextAccessor()
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration)
    .AddAuthenticationAndAuthorization();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddHealthChecks();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10)
            }));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler(opt => { });

app.MapEndpoints();

app.Run();

public partial class Program;