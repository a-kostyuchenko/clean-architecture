using SharedKernel.Errors;

namespace Domain.Users.Errors;

public static class AuthenticationErrors
{
    public static readonly Error UserNotFound = Error.NotFound(
        "Authentication.UserNotFound",
        "User with specified email was not found");
        
    public static readonly Error InvalidPassword = Error.Problem(
        "Authentication.InvalidCredentials",
        "The specified password is incorrect");
        
    public static readonly Error InvalidEmail = Error.Problem(
        "Authentication.InvalidEmail",
        "The specified email is incorrect");
}