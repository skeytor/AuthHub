namespace AuthHub.Infrastructure.Authorization;

[Flags]
public enum Permissions
{
    None = 0,
    CanViewRoles = 1,
    CanManageRoles = 2,
    CanViewUsers = 4,
    CanManageUsers = 8,
    Forecast = 16,
    All = int.MaxValue,
}
