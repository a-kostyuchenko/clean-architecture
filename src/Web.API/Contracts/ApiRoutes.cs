namespace Web.API.Contracts;

internal static class ApiRoutes
{
    internal static class Authentication
    {
        internal const string Tag = "Authentication";
        internal const string BaseUri = "authentication";
        internal const string Login = "login";
    }
    
    internal static class Users
    {
        internal const string Tag = "Users";
        internal const string BaseUri = "users";
        internal const string ResourceId = "userId";
        internal const string ChangePassword = $"{{{ResourceId}:guid}}/change-password";
        internal const string GetById = $"{{{ResourceId}:guid}}";
    }

    internal static class Health
    {
        internal const string BaseUri = "health";
    }
}