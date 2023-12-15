using Carter;
using Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("Database");
        
        Ensure.NotNullOrWhiteSpace(connection);
        
        services.AddCarter();

        services
            .AddHealthChecks()
            .AddNpgSql(connection!);
            
        return services;
    }
}