using AuthHub.Domain.Entities;

namespace App.Persistence.IntegrationTest.Initialization;

public static class SampleData
{
    public static List<Role> Roles =>
    [
        new() { Id = 1, Name = "Administrator", Description = "This is an admins user"},
        new() { Id = 2, Name = "Supervisor", Description = "This is a supervisor user"},
    ];

    public static List<User> Users =>
    [
        new() { FirstName = "Robert", LastName = "Guerrero", Email = "rober@email.com", IsActive = true, Password = "StrongPassword!12", Username = "rober_1", RoleId = 1 },
        new() { FirstName = "Paul", LastName = "Smith", Email = "paul@email.com", IsActive = true, Password = "StrongPassword!12", Username = "paul_1", RoleId = 2 },
        new() { FirstName = "Carlos", LastName = "Santana", Email = "carlos@email.com", IsActive = false, Password = "StrongPassword!12", Username = "carlos_1", RoleId = 1 },
        new() { FirstName = "Joseph", LastName = "Perez", Email = "joseph@email.com", IsActive = true, Password = "StrongPassword!12", Username = "joseph_1", RoleId = 2 },
        new() { FirstName = "Will", LastName = "Jhonson", Email = "will@email.com", IsActive = true, Password = "StrongPassword!12", Username = "will_1", RoleId = 1 },

    ];
}
