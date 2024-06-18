using Domain.Users;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public class PermissionAuthorizationHandler 
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        HashSet<string> permissions = context.User.GetPermissions();

        if (permissions.Any(p => p == requirement.Permission || p == AdministratorPermission.AccessEverything))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}
