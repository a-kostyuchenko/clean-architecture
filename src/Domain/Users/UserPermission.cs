using SharedKernel;

namespace Domain.Users;

public sealed class UserPermission(int id, string name, string description) 
    : Permission(id, name, description)
{
    public override string Key => "Users";

    public static readonly Permission Read = new UserPermission(1, nameof(Read), "Can read user information");
    public static readonly Permission Write = new UserPermission(2, nameof(Write), "Can write user information");
}