using Microsoft.AspNetCore.Authorization;

namespace AuthHub.Infrastructure.Authentication;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
