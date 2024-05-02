using Application.Abstractions;
using Application.Abstractions.Caching;
using Application.Abstractions.Cryptography;
using Domain.Users;
using FluentValidation;
using Infrastructure.Authorization;
using Infrastructure.Caching;
using Infrastructure.Common;
using Infrastructure.Cryptography;
using Infrastructure.Extensions;
using Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddScoped<IUserContext, UserContext>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();

        services.ConfigureOptions<OutboxBackgroundJobSetup>();

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<JwtOptions>(JwtOptions.ConfigurationSection);

        services.AddOptionsWithFluentValidation<OutboxOptions>(OutboxOptions.ConfigurationSection);
        
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        services.ConfigureOptions<JsonOptionsSetup>();
        
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