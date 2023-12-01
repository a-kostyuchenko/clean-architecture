namespace Infrastructure.Outbox;

public class OutboxOptions
{
    public int BatchSize { get; init; }
    public int IntervalInSeconds { get; init; }
    public int RetriesCount { get; init; }
}