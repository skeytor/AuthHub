using AuthHub.Domain.Entities;
using AuthHub.Infrastructure.Authorization;
using FluentAssertions;

namespace Api.UnitTest.Systems.Services.Permissiones;

public class PermissionServiceTest
{
    [Fact]
    public void Get_Permissions()
    {
        var permissions = Enum.GetValues<Permissions>()
            .Where(x => x != Permissions.None && x != Permissions.All)
            .Select((x, i) => new Permission
            {
                Id = i,
                Name = x.ToString()
            });
        permissions.First().Should().NotBe("None");
        permissions.Last().Should().NotBe("All");
    }
}
