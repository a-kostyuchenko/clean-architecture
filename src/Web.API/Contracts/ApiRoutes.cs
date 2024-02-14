namespace Web.API.Contracts;

public static class ApiRoutes
{
    public static class Authentication
    {
        public const string Base = "authentication";
        public const string Login = "login";
    }
    
    public static class Users
    {
        public const string Base = "users";
        public const string ChangePassword = "{userId:guid}/change-password";
    }

    public static class Health
    {
        public const string Base = "health";
    }
}