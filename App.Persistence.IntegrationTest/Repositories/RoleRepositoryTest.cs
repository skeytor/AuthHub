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
            List<Permission> permissions =
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
            Context.Permissions.AddRange(permissions);
            Role role = new()
            {
                Name = "Admin",
                Description = "This is a admin user",
                Permissions = permissions
            };
            Context.Roles.Add(role);
            Context.SaveChanges();
            Assert.Equal(1, role.Id);
        }
    }
}
