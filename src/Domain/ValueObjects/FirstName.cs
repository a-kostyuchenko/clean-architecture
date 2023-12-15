using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed record FirstName
{
    public const int MaxLength = 100;
    
    private FirstName(string value) => Value = value;
    
    public string Value { get; }

    public static implicit operator string(FirstName firstName) => firstName.Value;
    
    public static Result<FirstName> Create(string firstName) =>
        Result.Create(firstName, DomainErrors.FirstName.NullOrEmpty)
            .Ensure(f => !string.IsNullOrWhiteSpace(f), DomainErrors.FirstName.NullOrEmpty)
            .Ensure(f => f.Length <= MaxLength, DomainErrors.FirstName.TooLong)
            .Map(f => new FirstName(f));

    public override string ToString() => Value;
}