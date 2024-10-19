using AuthHub.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AuthHub.Infrastructure.Authorization;

public class CustomClaimsTransformation(IUserRepository userRepository) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new();
        if (principal.HasClaim(c => c.Type == CustomClaimTypes.Permissions))
        {
            return principal;
        }

        string? identityId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(identityId, out var userId))
        {
            return principal;
        }
        HashSet<string> permissions = await userRepository.GetPermissions(userId);
        var userPermissions = Permissions.None;
        foreach (var permission in permissions)
        {
            userPermissions |= Enum.Parse<Permissions>(permission);
        }
        int permissionsValue = (int)userPermissions;
        claimsIdentity.AddClaim(new Claim(CustomClaimTypes.Permissions, permissionsValue.ToString()));
        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}
