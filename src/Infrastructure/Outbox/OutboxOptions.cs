namespace Infrastructure.Outbox;

public class OutboxOptions
{
    public const string ConfigurationSection = "Outbox";
    
    public int BatchSize { get; init; }
    public string Schedule { get; init; }
}
