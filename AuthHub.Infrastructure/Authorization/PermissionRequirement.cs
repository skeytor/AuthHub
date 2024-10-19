using AuthHub.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace AuthHub.Infrastructure.Authorization;

public class PermissionRequirement(Permissions permission) : IAuthorizationRequirement
{
    public Permissions Permissions { get; } = permission;
}
