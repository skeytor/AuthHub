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
            List<Permission> permissions1 =
            [
                new Permission
                {
                    Name = "CanManageUser",
                },
                new Permission
                {
                    Name = "CanViewUser"
                }
            ];
            Context.Permissions.AddRange(permissions1 );
            Context.SaveChanges();
            List<Permission> permissions2 = permissions1.Select(x => new Permission { Id = x.Id} ).ToList();
            Role role = new()
            {
                Name = "Admin",
                Description = "This is a admin user",
                Permissions = permissions2
            };
            Context.Roles.Add(role);
            Context.SaveChanges();
            var s = Context.RolePermissions.ToList();
            Assert.Equal(1, role.Id);
        }
    }
}
