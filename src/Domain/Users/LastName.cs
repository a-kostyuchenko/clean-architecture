using SharedKernel;

namespace Domain.Users;

public record LastName
{
    public const int MaxLength = 100;
    
    private LastName(string value) => Value = value;
    
    public string Value { get; }

    public static implicit operator string(LastName lastName) => lastName.Value;
    
    public static Result<LastName> Create(string lastName) =>
        Result.Create(lastName, LastNameErrors.NullOrEmpty)
            .Ensure(l => !string.IsNullOrWhiteSpace(l), LastNameErrors.NullOrEmpty)
            .Ensure(l => l.Length <= MaxLength, LastNameErrors.TooLong)
            .Map(l => new LastName(l));
        
    public override string ToString() => Value;
}