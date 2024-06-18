using SharedKernel.Errors;

namespace Domain.Users.Errors;

public static class PasswordErrors
{
    public static readonly Error NullOrEmpty = 
        Error.Problem("Password.NullOrEmpty", "The password is required");

    public static readonly Error TooShort = 
        Error.Problem("Password.TooShort", "The password is too short");

    public static readonly Error MissingUppercaseLetter = Error.Problem(
        "Password.MissingUppercaseLetter",
        "The password requires at least one uppercase letter");

    public static readonly Error MissingLowercaseLetter = Error.Problem(
        "Password.MissingLowercaseLetter",
        "The password requires at least one lowercase letter");

    public static readonly Error MissingDigit = Error.Problem(
        "Password.MissingDigit",
        "The password requires at least one digit");

    public static readonly Error MissingNonAlphaNumeric = Error.Problem(
        "Password.MissingNonAlphaNumeric",
        "The password requires at least one non-alphanumeric");
}