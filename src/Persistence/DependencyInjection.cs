using Application.Abstractions.Data;
using Application.Abstractions.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Persistence.Idempotency;
using Persistence.Interceptors;
using SharedKernel;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("Database");
        
        Ensure.NotNullOrWhiteSpace(connection);

        services.AddSingleton<UpdateDeletableInterceptor>();
        services.AddSingleton<UpdateAuditableInterceptor>();
        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connection);

            options.AddInterceptors(
                sp.GetRequiredService<UpdateDeletableInterceptor>(),
                sp.GetRequiredService<UpdateAuditableInterceptor>(),
                sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
        });

        services.AddSingleton(_ =>
            new DbConnectionFactory(new NpgsqlDataSourceBuilder(connection).Build()));
        
        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IIdempotencyService, IdempotencyService>();
        
        return services;
    }
}