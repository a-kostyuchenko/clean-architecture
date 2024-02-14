using SharedKernel;

namespace Domain.Users;

public static class FirstNameErrors
{
    public static readonly Error NullOrEmpty = 
        Error.Problem("FirstName.NullOrEmpty", "The first name is required");

    public static readonly Error TooLong = 
        Error.Problem("FirstName.TooLong", "The first name is longer than allowed");
}