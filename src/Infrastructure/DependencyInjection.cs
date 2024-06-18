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
        services.AddSingleton<ISystemTimeProvider, SystemTimeProvider>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddScoped<IUserContext, UserContext>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();
        
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<JwtOptions>(JwtOptions.ConfigurationSection);

        services.AddOptionsWithFluentValidation<OutboxOptions>(OutboxOptions.ConfigurationSection);
        
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        services.ConfigureOptions<JsonOptionsSetup>();
        
        string redisConnectionString = configuration.GetConnectionString("Cache")!;

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);
        
        services.AddSingleton<ICacheService, CacheService>();
        
        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(
                options => options.UseNpgsqlConnection(configuration.GetConnectionString("Database"))));

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();
        
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

        services.AddScoped<IIdempotencyService, IdempotencyService>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthorization();

        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        
        return services;
    }
}
