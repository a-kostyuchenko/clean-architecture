using SharedKernel;

namespace Domain.Users.Permissions;

public sealed class UserPermission(int id, string name, string description) 
    : ApplicationPermission(id, name, description)
{
    public override string Key => "Users";

    public static readonly ApplicationPermission Read = new UserPermission(1, nameof(Read), "Can read user information");
    public static readonly ApplicationPermission Write = new UserPermission(2, nameof(Write), "Can write user information");
}