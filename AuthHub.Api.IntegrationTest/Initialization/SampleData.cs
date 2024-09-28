using AuthHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.IntegrationTest.Initialization;

public static class SampleData
{
    private readonly static PasswordHasher<User> passwordHasher = new();
    public static List<Permission> Permissions =>
    [
        new Permission { Id = 1, Name = "CanRead" },
        new Permission { Id = 2, Name = "CanDelete" },
        new Permission { Id = 3, Name = "CanView" },
        new Permission { Id = 4, Name = "CanUpdate" }
    ];

    public static List<Role> Roles =>
    [
        new() { Id = 1, Name = "Admin", Description = "This is an admins user" },
        new() { Id = 2, Name = "Supervisor", Description = "This is a supervisor user" },
    ];


    public static List<User> Users => GetUsers();
    private static List<User> GetUsers()
    {
        List<User> users =
        [
            new() { FirstName = "Robert", LastName = "Guerrero", Email = "rober@email.com", IsActive = true, Password = "StrongPassword!12", Username = "rober_1", RoleId = 1 },
            new() { FirstName = "Paul", LastName = "Smith", Email = "paul@email.com", IsActive = true, Password = "StrongPassword!12", Username = "paul_1", RoleId = 2 },
            new() { FirstName = "Carlos", LastName = "Santana", Email = "carlos@email.com", IsActive = true, Password = "StrongPassword!12", Username = "carlos_1", RoleId = 1 },
            new() { FirstName = "Joseph", LastName = "Perez", Email = "joseph@email.com", IsActive = true, Password = "StrongPassword!12", Username = "joseph_1", RoleId = 2 },
            new() { FirstName = "Will", LastName = "Jhonson", Email = "will@email.com", IsActive = true, Password = "StrongPassword!12", Username = "will_1", RoleId = 1 },
        ];
        return users
            .Select(x =>
            {
                x.Password = passwordHasher.HashPassword(x, x.Password);
                return x;
            })
            .ToList();
    }
}
