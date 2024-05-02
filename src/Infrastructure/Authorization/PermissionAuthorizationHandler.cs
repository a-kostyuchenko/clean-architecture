using Domain.Users;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public class PermissionAuthorizationHandler 
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var permissions = context
            .User
            .Claims
            .Where(x => x.Type == CustomClaims.Permissions)
            .Select(x => x.Value)
            .ToHashSet();

        if (permissions.Any(p => p == requirement.Permission || p == AdministratorPermission.AccessEverything))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}