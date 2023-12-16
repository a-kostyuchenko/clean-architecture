namespace Infrastructure.Outbox;

public class OutboxOptions
{
    public const string ConfigurationSection = "Outbox";
    
    public int BatchSize { get; init; }
    public int IntervalInSeconds { get; init; }
    public int RetriesCount { get; init; }
}