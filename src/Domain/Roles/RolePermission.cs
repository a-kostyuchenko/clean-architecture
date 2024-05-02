namespace Domain.Roles;

public sealed class RolePermission
{
    public int RoleId { get; init; }

    public Guid PermissionId { get; init; }
}