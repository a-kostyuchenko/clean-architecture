using Application.Abstractions;

namespace Infrastructure.Services;

public class SystemTimeProvider : ISystemTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}