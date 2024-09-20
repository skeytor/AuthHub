using AuthHub.Api.Controllers;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Roles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.UnitTest.Systems.Controllers.Roles;

public class RoleControllerTest
{
    [Fact]
    public async Task Create_Should_ReturnRoleName_WhenRequestDataIsValid()
    {
        // Arrange
        CreateRoleRequest request = new("Admin", "This is an admin user", []);

        Mock<IRoleService> mockRoleService = new();
        mockRoleService
            .Setup(service => service.CreateAsync(It.IsAny<CreateRoleRequest>()))
            .ReturnsAsync(request.Name)
            .Verifiable(Times.Once());

        RoleController controller = new(mockRoleService.Object);

        // Act
        var response = await controller.Create(request);

        // Assert
        mockRoleService.Verify();
        response.Should().BeOfType<CreatedAtActionResult>();

        var objResult = (CreatedAtActionResult)response;
        objResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        objResult.Value.Should().BeOfType<string>();
        
        string roleName = (string)objResult.Value!;
        roleName.Should().Be(request.Name);
    }

    [Fact]
    public async Task GetAll_Should_ReturnRoleList_WhenRolesExist()
    {
        // Arrange
        List<RoleResponse> rolesExpected =
        [
            new(1, "Admin", "Desc", ["CanRead", "CanDelete"]),
            new(2, "Supervisor", "Desc", ["CanView"]),
            new(3, "SuperUser", "Desc", ["CanRead", "CanDelete", "CanView"]),
        ];
        Mock<IRoleService> mockRoleService = new();
        mockRoleService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(rolesExpected);


        RoleController controller = new(mockRoleService.Object);

        // Act
        var response = await controller.GetAll();

        // Assert
        mockRoleService.Verify();
        response.Should().BeOfType<OkObjectResult>();

        var objResult = (OkObjectResult)response;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        objResult.Value.Should().BeAssignableTo<IReadOnlyList<RoleResponse>>();
        
        IReadOnlyList<RoleResponse> roles = (IReadOnlyList<RoleResponse>)objResult.Value!;
        roles.Should().NotBeNullOrEmpty();
        roles.Should().HaveSameCount(rolesExpected);

    }
}
