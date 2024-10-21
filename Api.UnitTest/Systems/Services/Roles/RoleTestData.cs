using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;

namespace Api.UnitTest.Systems.Services.Roles;

internal class RoleTestData
{
    internal class ValidUpdateRoleTestData : TheoryData<List<Permission>, Role, CreateRoleRequest, int>
    {
        public ValidUpdateRoleTestData() 
        {
            Add(
                [
                    new() { Id = 1, Name = "Permission1"},
                    new() { Id = 2, Name = "Permission2" }
                ],
                new()
                {
                    Id = 1,
                    Name = "RoleTest1",
                    Permissions = 
                    [
                        new() { Id = 1, Name = "Permission1" }, 
                        new() { Id = 2, Name = "Permission2"}
                    ]
                },
                new("Role1Changed", "This role was changed", [2]),
                1
            );
            Add(
                [
                    new() { Id = 1, Name = "Permission1"},
                    new() { Id = 2, Name = "Permission2"},
                    new() { Id = 3, Name = "Permission3"},
                    new() { Id = 4, Name = "Permission4"},
                    new() { Id = 5, Name = "Permission5" }
                ],
                new()
                {
                    Id = 1,
                    Name = "RoleTest2",
                    Permissions = 
                    [
                        new() { Id = 1, Name = "Permission1" }, 
                        new() { Id = 2, Name = "Permission2" }
                    ]
                },
                new("Role2Changed", "This role was changed", [2, 3, 4]),
                1
            );
        }
    }
}
