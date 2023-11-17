using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public record Password
{
    private const int MinPasswordLength = 8;
    private static readonly Func<char, bool> IsLower = c => c is >= 'a' and <= 'z';
    private static readonly Func<char, bool> IsUpper = c => c is >= 'A' and <= 'Z';
    private static readonly Func<char, bool> IsDigit = c => c is >= '0' and <= '9';
    private static readonly Func<char, bool> IsNonAlphaNumeric = c => !(IsLower(c) || IsUpper(c) || IsDigit(c));
    
    private Password(string value) => Value = value;
    
    public string Value { get; }

    public static implicit operator string(Password password) => password?.Value ?? string.Empty;
    
    public static Result<Password> Create(string password) =>
        Result.Create(password, DomainErrors.Password.NullOrEmpty)
            .Ensure(p => !string.IsNullOrWhiteSpace(p), DomainErrors.Password.NullOrEmpty)
            .Ensure(p => p.Length >= MinPasswordLength, DomainErrors.Password.TooShort)
            .Ensure(p => p.Any(IsLower), DomainErrors.Password.MissingLowercaseLetter)
            .Ensure(p => p.Any(IsUpper), DomainErrors.Password.MissingUppercaseLetter)
            .Ensure(p => p.Any(IsDigit), DomainErrors.Password.MissingDigit)
            .Ensure(p => p.Any(IsNonAlphaNumeric), DomainErrors.Password.MissingNonAlphaNumeric)
            .Map(p => new Password(p));
}