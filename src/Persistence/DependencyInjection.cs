using Application.Abstractions.Data;
using Application.Abstractions.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        string? connection = configuration.GetConnectionString("Database");
        
        Ensure.NotNullOrWhiteSpace(connection);

        services.AddSingleton<UpdateDeletableInterceptor>();
        services.AddSingleton<UpdateAuditableInterceptor>();
        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connection).UseSnakeCaseNamingConvention();

            options.AddInterceptors(
                sp.GetRequiredService<UpdateDeletableInterceptor>(),
                sp.GetRequiredService<UpdateAuditableInterceptor>(),
                sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
        });

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(connection).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();
        
        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IIdempotencyService, IdempotencyService>();
        
        return services;
    }
}
