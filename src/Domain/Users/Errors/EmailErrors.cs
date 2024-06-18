using SharedKernel.Errors;

namespace Domain.Users.Errors;

public static class EmailErrors
{
    public static readonly Error NullOrEmpty = 
        Error.Problem("Email.NullOrEmpty", "The email is required");

    public static readonly Error TooLong =
        Error.Problem("Email.TooLong", "The email is longer than allowed");

    public static readonly Error InvalidFormat = 
        Error.Problem("Email.InvalidFormat", "The email format is invalid");
}