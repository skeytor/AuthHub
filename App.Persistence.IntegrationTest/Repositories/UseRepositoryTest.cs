using AuthHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.IntegrationTest.Repositories;

public class UseRepositoryTest(MsSqlContainerFixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task InsertAsync_Should_ReturnSuccess()
    {
        var users = await Context.Users.ToListAsync();
        var roles = await Context.Roles.ToListAsync();
        Assert.IsType<List<User>>(users);
    }
}
