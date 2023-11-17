using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public record LastName
{
    public const int MaxLength = 100;
    
    private LastName(string value) => Value = value;
    
    public string Value { get; }

    public static implicit operator string(LastName lastName) => lastName.Value;
    
    public static Result<LastName> Create(string lastName) =>
        Result.Create(lastName, DomainErrors.LastName.NullOrEmpty)
            .Ensure(l => !string.IsNullOrWhiteSpace(l), DomainErrors.LastName.NullOrEmpty)
            .Ensure(l => l.Length <= MaxLength, DomainErrors.LastName.TooLong)
            .Map(l => new LastName(l));
        
    public override string ToString() => Value;
}