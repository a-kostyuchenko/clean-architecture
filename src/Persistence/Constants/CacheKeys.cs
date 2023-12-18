namespace Persistence.Constants;

public static class CacheKeys
{
    public static readonly string Users = "users";
    public static readonly Func<Guid, string> UserById = userId => $"user-{userId}";
}