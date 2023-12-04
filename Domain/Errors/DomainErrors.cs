using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Email
    {
        public static readonly Error NullOrEmpty = 
            new("Email.NullOrEmpty", "The email is required");

        public static readonly Error TooLong =
            new("Email.TooLong", "The email is longer than allowed");

        public static readonly Error InvalidFormat = 
            new("Email.InvalidFormat", "The email format is invalid");
    }
    
    public static class FirstName
    {
        public static readonly Error NullOrEmpty = 
            new("FirstName.NullOrEmpty", "The first name is required");

        public static readonly Error TooLong = 
            new("FirstName.TooLong", "The first name is longer than allowed");
    }
    
    public static class LastName
    {
        public static readonly Error NullOrEmpty = 
            new("LastName.NullOrEmpty", "The last name is required");

        public static readonly Error TooLong = 
            new("LastName.TooLong", "The last name is longer than allowed");
    }
    
    public static class Password
    {
        public static readonly Error NullOrEmpty = 
            new("Password.NullOrEmpty", "The password is required");

        public static readonly Error TooShort = 
            new("Password.TooShort", "The password is too short");

        public static readonly Error MissingUppercaseLetter = new(
            "Password.MissingUppercaseLetter",
            "The password requires at least one uppercase letter");

        public static readonly Error MissingLowercaseLetter = new(
            "Password.MissingLowercaseLetter",
            "The password requires at least one lowercase letter");

        public static readonly Error MissingDigit = new(
            "Password.MissingDigit",
            "The password requires at least one digit");

        public static readonly Error MissingNonAlphaNumeric = new(
            "Password.MissingNonAlphaNumeric",
            "The password requires at least one non-alphanumeric");
    }
    
    public static class User
    {
        public static readonly Error CannotChangePassword = new(
            "User.CannotChangePassword",
            "The password cannot be changed to the specified password.");
        
        public static readonly Error NotFound = new(
            "User.NotFound",
            "The user with the specified identifier was not found.");

        public static readonly Error InvalidPermissions = new(
            "User.InvalidPermissions",
            "The current user does not have the permissions to perform that operation.");
    }
    
    public static class Authentication
    {
        public static readonly Error UserNotFound = new(
            "Authentication.UserNotFound",
            "User with specified email was not found");
        
        public static readonly Error InvalidPassword = new(
            "Authentication.InvalidCredentials",
            "The specified password is incorrect");
        
        public static readonly Error InvalidEmail = new(
            "Authentication.InvalidEmail",
            "The specified email is incorrect");
    }
}