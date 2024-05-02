using SharedKernel;

namespace Domain.Users;

public sealed class AdministratorPermission(int id, string name, string description) 
    : Permission(id, name, description)
{
    public override string Key => "Administrator";

    public static readonly Permission AccessEverything = 
        new AdministratorPermission(int.MaxValue, nameof(AccessEverything), "Can access everything");
}