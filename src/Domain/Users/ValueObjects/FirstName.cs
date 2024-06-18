using Domain.Users.Errors;
using SharedKernel.Result;

namespace Domain.Users.ValueObjects;

public sealed record FirstName
{
    public const int MaxLength = 100;
    
    private FirstName(string value) => Value = value;
    
    public string Value { get; }

    public static implicit operator string(FirstName firstName) => firstName.Value;
    
    public static Result<FirstName> Create(string firstName) =>
        Result.Create(firstName, FirstNameErrors.NullOrEmpty)
            .Ensure(f => !string.IsNullOrWhiteSpace(f), FirstNameErrors.NullOrEmpty)
            .Ensure(f => f.Length <= MaxLength, FirstNameErrors.TooLong)
            .Map(f => new FirstName(f));

    public override string ToString() => Value;
}