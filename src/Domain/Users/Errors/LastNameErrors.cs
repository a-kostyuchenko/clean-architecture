using SharedKernel.Errors;

namespace Domain.Users.Errors;

public static class LastNameErrors
{
    public static readonly Error NullOrEmpty = 
        Error.Problem("LastName.NullOrEmpty", "The last name is required");

    public static readonly Error TooLong = 
        Error.Problem("LastName.TooLong", "The last name is longer than allowed");
}