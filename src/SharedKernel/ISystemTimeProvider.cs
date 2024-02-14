namespace SharedKernel;

public interface ISystemTimeProvider
{
    public DateTime UtcNow { get; }
}