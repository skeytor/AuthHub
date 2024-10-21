using AuthHub.Api.Controllers;
using AuthHub.Api.Services.Permissions;
using AuthHub.Domain.Entities;
using AuthHub.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.UnitTest.Systems.Controllers.PermissionsTest;

public class PermissionControllerTest
{
    [Fact]
    public async Task GetAllAsync_Should_ReturnAllPermission_WhenInvokePermissionService()
    {
        // Arrange
        var permissions = Enum.GetValues<Permissions>()
             .Where(x => x != Permissions.All && x != Permissions.None)
             .Select(x => new Permission { Id = (int)x, Name = x.ToString() })
             .ToList();
        Mock<IPermissionService> mockPermissionService = new();
        mockPermissionService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(permissions)
            .Verifiable(Times.Once());

        PermissionController controller = new(mockPermissionService.Object);

        // Act
        var response = await controller.GetAll();

        // Assert
        mockPermissionService.Verify();
        response.Should().BeOfType<OkObjectResult>();

        OkObjectResult objectResult = (OkObjectResult)response;
        objectResult.Value.Should().NotBeNull();
        objectResult.Value.Should().BeAssignableTo<IReadOnlyList<Permission>>();

        var data = (IReadOnlyList<Permission>)objectResult.Value!;

        data.Should().HaveSameCount(permissions);
    }
}
