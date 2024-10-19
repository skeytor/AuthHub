using Microsoft.AspNetCore.Authorization;

namespace AuthHub.Infrastructure.Authorization;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public CustomAuthorizeAttribute() { }
    public CustomAuthorizeAttribute(string policy) : base(policy) { }
    public CustomAuthorizeAttribute(Permissions permissions) => Permissions = permissions;

    public Permissions Permissions
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Policy)
                ? PolicyNameHelper.GetPermissionsFrom(Policy)
                : Permissions.None;
        }
        set
        {
            Policy = value != Permissions.None
                ? PolicyNameHelper.GeneratePolicyNameFor(value)
                : string.Empty;
        }
    }
}
