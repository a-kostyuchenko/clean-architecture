using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error CannotChangePassword = Error.Problem(
        "User.CannotChangePassword",
        "The password cannot be changed to the specified password.");
        
    public static readonly Error NotFound = Error.NotFound(
        "User.NotFound",
        "The user with the specified identifier was not found.");

    public static readonly Error InvalidPermissions = Error.Problem(
        "User.InvalidPermissions",
        "The current user does not have the permissions to perform that operation.");
        
    public static readonly Error EmailAlreadyInUse = Error.Conflict(
        "User.EmailAlreadyInUse",
        "The specified email is already in use");
}