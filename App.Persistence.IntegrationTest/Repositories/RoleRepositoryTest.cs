using AuthHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace App.Persistence.IntegrationTest.Repositories;

public class RoleRepositoryTest(
    MsSqlContainerFixture fixture,
    ITestOutputHelper outputHelper) : BaseTest(fixture, outputHelper)
{
    [Fact]
    public void InsertRole_Should_ReturnSuccess()
    {
        ExecuteInATransaction(RunTest);
        void RunTest()
        {
            Role role = new()
            {
                Name = "Admin",
                Description = "This is a admin user",
                Permissions = [new() { Name = "CanUserRead" }, new() { Name = "CanUserUpdate" }]
            };
            User user = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "email@email.com",
                Password = "pass",
                IsActive = true,
                Role = role,
                Username = "user_name"
            };
            Context.Roles.Add(role);
            Context.Users.Add(user);
            Context.SaveChanges();
            Role[] roles = Context
                .Users
                .Include(x => x.Role)
                .ThenInclude(x => x.Permissions)
                .Where(x => x.Id == user.Id)
                .Select(x => x.Role)
                .ToArray();
            IReadOnlySet<string> permissions = roles
                .SelectMany(x => x.Permissions)
                .Select(x => x.Name)
                .ToHashSet();
            Assert.Equal(user.Role.Permissions.Count, permissions.Count);
        }
    }
}
