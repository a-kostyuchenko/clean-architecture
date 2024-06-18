using System.Text.RegularExpressions;
using Domain.Users.Errors;
using SharedKernel.Result;

namespace Domain.Users.ValueObjects;

public sealed record Email
{
    public const int MaxLength = 256;

    private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    private static readonly Lazy<Regex> EmailFormatRegex =
        new (() => new Regex(EmailRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));
    
    private Email(string value) => Value = value;
    
    public string Value { get; }

    public static implicit operator string(Email email) => email.Value;
    
    public static Result<Email> Create(string email) =>
        Result.Create(email, EmailErrors.NullOrEmpty)
            .Ensure(e => !string.IsNullOrWhiteSpace(e), EmailErrors.NullOrEmpty)
            .Ensure(e => e.Length <= MaxLength, EmailErrors.TooLong)
            .Ensure(e => EmailFormatRegex.Value.IsMatch(e), EmailErrors.InvalidFormat)
            .Map(e => new Email(e));
}