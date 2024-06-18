using Application.Abstractions;
using Application.Abstractions.Caching;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Data;
using Application.Abstractions.Idempotency;
using Domain.Users;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Caching;
using Infrastructure.Clock;
using Infrastructure.Cryptography;
using Infrastructure.Database;
using Infrastructure.Database.Interceptors;
using Infrastructure.Extensions;
using Infrastructure.Idempotency;
using Infrastructure.Outbox;
using Infrastructure.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string databaseConnection = configuration.GetConnectionString("Database")!;
        string redisConnection = configuration.GetConnectionString("Cache")!;
        
        Ensure.NotNullOrWhiteSpace(databaseConnection);
        Ensure.NotNullOrWhiteSpace(redisConnection);

        services.AddSingleton<ISystemTimeProvider, SystemTimeProvider>();
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);
        services.ConfigureOptions<JsonOptionsSetup>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        
        AddDatabase(services, databaseConnection);
        AddBackgroundJobs(services, databaseConnection);
        AddCaching(services, redisConnection);
        AddAuthenticationAndAuthorization(services);
        AddHealthChecks(services, databaseConnection, redisConnection);
        
        return services;
    }

    private static void AddDatabase(IServiceCollection services, string databaseConnection)
    {
        services.AddSingleton<UpdateDeletableInterceptor>();
        services.AddSingleton<UpdateAuditableInterceptor>();
        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(databaseConnection).UseSnakeCaseNamingConvention();

            options.AddInterceptors(
                sp.GetRequiredService<UpdateDeletableInterceptor>(),
                sp.GetRequiredService<UpdateAuditableInterceptor>(),
                sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
        });

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnection).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();
        
        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());
    }

    private static void AddBackgroundJobs(IServiceCollection services, string databaseConnection)
    {
        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(
                options => options.UseNpgsqlConnection(databaseConnection)));
        
        services.AddOptionsWithFluentValidation<OutboxOptions>(OutboxOptions.ConfigurationSection);

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();
    }

    private static void AddCaching(IServiceCollection services, string redisConnection)
    {
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnection);
        
        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddAuthenticationAndAuthorization(IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddScoped<IUserContext, UserContext>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();
        
        services.AddOptionsWithFluentValidation<JwtOptions>(JwtOptions.ConfigurationSection);
        
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthorization();
        
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }
    
    private static void AddHealthChecks(
        IServiceCollection services,
        string databaseConnection,
        string redisConnection)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(databaseConnection)
            .AddRedis(redisConnection)
            .AddDbContextCheck<ApplicationDbContext>();
    }
}
