using SharedKernel;

namespace Domain.Users;

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
        Result.Create(password, PasswordErrors.NullOrEmpty)
            .Ensure(p => !string.IsNullOrWhiteSpace(p), PasswordErrors.NullOrEmpty)
            .Ensure(p => p.Length >= MinPasswordLength, PasswordErrors.TooShort)
            .Ensure(p => p.Any(IsLower), PasswordErrors.MissingLowercaseLetter)
            .Ensure(p => p.Any(IsUpper), PasswordErrors.MissingUppercaseLetter)
            .Ensure(p => p.Any(IsDigit), PasswordErrors.MissingDigit)
            .Ensure(p => p.Any(IsNonAlphaNumeric), PasswordErrors.MissingNonAlphaNumeric)
            .Map(p => new Password(p));
}