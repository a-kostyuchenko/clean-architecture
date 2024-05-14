using SharedKernel;

namespace Infrastructure.Clock;

public class SystemTimeProvider : ISystemTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
