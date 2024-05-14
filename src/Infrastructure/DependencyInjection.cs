using Application.Abstractions;
using Application.Abstractions.Caching;
using Application.Abstractions.Cryptography;
using Domain.Users;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Authorization;
using Infrastructure.Caching;
using Infrastructure.Clock;
using Infrastructure.Cryptography;
using Infrastructure.Extensions;
using Infrastructure.Outbox;
using Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();

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
        
        return services;
    }
}
