using Application.Abstractions;
using Application.Abstractions.Cryptography;
using Domain.Services;
using Infrastructure.Common;
using Infrastructure.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();
        
        return services;
    }
}