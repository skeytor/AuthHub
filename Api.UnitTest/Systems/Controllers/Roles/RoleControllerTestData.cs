using AuthHub.Api.Dtos;

namespace Api.UnitTest.Systems.Controllers.Roles;

internal class RoleControllerTestData
{
    internal class ValidUpdateRoleControllerTestData : TheoryData<int, CreateRoleRequest, string>
    {
        public ValidUpdateRoleControllerTestData() 
        { 
            Add(1, new CreateRoleRequest("Role1Changed", "This role was changed", [1, 3, 4]), "Role1Changed");
            Add(1, new CreateRoleRequest("Role2Changed", "This role was changed", []), "Role2Changed");
            Add(1, new CreateRoleRequest("Role3Changed", "This role was changed", [1]), "Role3Changed");
            Add(1, new CreateRoleRequest("Role4Changed", "This role was changed", [3, 2]), "Role4Changed");
        }
    }
}
