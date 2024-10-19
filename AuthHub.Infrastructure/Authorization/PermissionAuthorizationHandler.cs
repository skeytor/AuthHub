using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthHub.Infrastructure.Authorization;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        Claim? permissionClaim = context.User.FindFirst(c => c.Type == CustomClaimTypes.Permissions);
        if (permissionClaim is null)
        {
            return Task.CompletedTask;
        }
        if (!int.TryParse(permissionClaim.Value, out int permissionClaimValue))
        {
            return Task.CompletedTask;
        }
        Permissions userPermissions = (Permissions)permissionClaimValue;
        if ((userPermissions & requirement.Permissions) != 0)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }
}
