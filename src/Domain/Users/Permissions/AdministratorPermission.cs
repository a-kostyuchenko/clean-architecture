using SharedKernel;

namespace Domain.Users.Permissions;

public sealed class AdministratorPermission(int id, string name, string description) 
    : ApplicationPermission(id, name, description)
{
    public override string Key => "Administrator";

    public static readonly ApplicationPermission AccessEverything = 
        new AdministratorPermission(int.MaxValue, nameof(AccessEverything), "Can access everything");
}
