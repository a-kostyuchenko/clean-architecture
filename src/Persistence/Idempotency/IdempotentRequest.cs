namespace Persistence.Idempotency;

internal sealed class IdempotentRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public DateTime CreatedOnUtc { get; init; }
}