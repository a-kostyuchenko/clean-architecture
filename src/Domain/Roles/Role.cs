using Domain.Users;
using SharedKernel;

namespace Domain.Roles;

public sealed class Role(int id, string name) : Enumeration<Role>(id, name)
{
    public static readonly Role Registered = new(1, "Registered");

    public ICollection<Permission> Permissions { get; set; }

    public ICollection<User> Users { get; set; }
}