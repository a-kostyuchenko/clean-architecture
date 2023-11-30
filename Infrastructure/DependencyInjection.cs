using Application.Abstractions;
using Application.Abstractions.Cryptography;
using Domain.Services;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.Common;
using Infrastructure.Cryptography;
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
        
        services.AddQuartz(configurator =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configurator
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger
                            .ForJob(jobKey)
                            .WithSimpleSchedule(schedule => schedule
                                        .WithIntervalInSeconds(10)
                                        .RepeatForever()));
        });

        services.AddQuartzHostedService();
        
        return services;
    }
}