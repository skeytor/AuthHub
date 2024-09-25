
using AuthHub.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace AuthHub.Infrastructure.Authentication;

public class PermissionAuthorizationHandler(IUserRepository userRepository)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        string? userIdClaimValue = context
            .User.
            Claims.
            FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (Guid.TryParse(userIdClaimValue, out var userId))
        {
            return;
        }

        HashSet<string> permissions = await userRepository.GetPermissions(userId);

        if (permissions.Contains(requirement.Permission)) 
        {
            context.Succeed(requirement);    
        }
    }
}
