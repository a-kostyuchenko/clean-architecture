using Domain.Users;
using SharedKernel;

namespace Domain.Roles;

public sealed class Role(int id, string name) : Enumeration<Role>(id, name)
{
    public static readonly Role Registered = new(1, "Registered");

    private readonly List<Permission> _permissions = [];
    private readonly List<User> _users = [];

    public List<Permission> Permissions => _permissions.ToList();
    public List<User> Users => _users.ToList();
}