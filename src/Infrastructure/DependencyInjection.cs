using Application.Abstractions;
using Application.Abstractions.Caching;
using Application.Abstractions.Cryptography;
using Domain.Services;
using FluentValidation;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.Common;
using Infrastructure.Cryptography;
using Infrastructure.Extensions;
using Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();

        services.ConfigureOptions<OutboxBackgroundJobSetup>();

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<JwtOptions>(JwtOptions.ConfigurationSection);

        services.AddOptionsWithFluentValidation<OutboxOptions>(OutboxOptions.ConfigurationSection);
        
        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        
        services.AddQuartz();
        
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        return services;
    }
}