using AuthHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.IntegrationTest.Initialization;

public static class SampleData
{
    private readonly static PasswordHasher<User> _passwordHasher = new();
    public static List<Permission> Permissions =>
    [
        new() { Id = 1, Name = "CanViewRoles" },
        new() { Id = 2, Name = "CanManageRoles" },
        new() { Id = 3, Name = "CanViewUsers" },
        new() { Id = 4, Name = "CanManageUsers" },
        new() { Id = 5, Name = "Forecast" },
    ];

    public static List<Role> Roles =>
    [
        new() { Id = 1, Name = "Admin", Description = "This is an admins user" },
        new() { Id = 2, Name = "Accounter", Description = "This is an accounter role" },
        new() { Id = 3, Name = "SuperAdmin", Description = "This is a super admin, it has all permissions" },
    ];
    public static List<RolePermission> RolePermissions =>
    [
        new() { PermissionId = 1, RoleId = 1},
        new() { PermissionId = 2, RoleId = 1},
        new() { PermissionId = 3, RoleId = 1},
        new() { PermissionId = 4, RoleId = 1},
        new() { PermissionId = 1, RoleId = 2},
        new() { PermissionId = 2, RoleId = 2},
        new() { PermissionId = 1, RoleId = 3},
        new() { PermissionId = 2, RoleId = 3},
        new() { PermissionId = 3, RoleId = 3},
        new() { PermissionId = 4, RoleId = 3},
        new() { PermissionId = 5, RoleId = 3},
    ];
    public static List<User> Users => GetUsers();
    private static List<User> GetUsers()
    {
        List<User> users =
        [
            new() { FirstName = "Robert", LastName = "Guerrero", Email = "rober@email.com", IsActive = true, Password = "StrongPassword!12", Username = "rober_1", RoleId = 1 },
            new() { FirstName = "Admin Test", LastName = "Admin Test", Email = "admin@email.com", IsActive = true, Password = "StrongPassword!12", Username = "admin_1", RoleId = 1 },
            new() { FirstName = "Accounter Test", LastName = "Accounter Test", Email = "accounter@email.com", IsActive = true, Password = "StrongPassword!12", Username = "accounter_1", RoleId = 2 },
            new() { FirstName = "Super Admin", LastName = "Super Admin Test", Email = "super_admin@email.com", IsActive = true, Password = "StrongPassword!12", Username = "super_admin_1", RoleId = 3 },
        ];
        return users.
            Select(x =>
            {
                x.Password = _passwordHasher.HashPassword(x, x.Password);
                return x;
            })
            .ToList();
    }
}
