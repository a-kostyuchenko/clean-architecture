using Application.Abstractions;
using Application.Abstractions.Cryptography;
using Domain.Services;
using Infrastructure.Authentication;
using Infrastructure.Common;
using Infrastructure.Cryptography;
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

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();

        services.ConfigureOptions<OutboxBackgroundJobSetup>();
        
        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        return services;
    }
}