using AuthHub.Api.Services.Permissions;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Infrastructure.Authorization;
using FluentAssertions;
using Moq;

namespace Api.UnitTest.Systems.Services.Permissiones;

public class PermissionServiceTest
{
    [Fact]
    public async void GetAllAsync_Should_ReturnAllPermissions()
    {
        // Arrange
        List<Permission> sampleData = Enum.GetValues<Permissions>()
            .Where(x => x != Permissions.None && x != Permissions.All)
            .Select(x => new Permission
            {
                Id = (int)x,
                Name = x.ToString()
            })
            .ToList();

        Mock<IPermissionRepository> mockPermissionRepository = new();
        mockPermissionRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(sampleData)
            .Verifiable(Times.Once());

        PermissionService service = new(mockPermissionRepository.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        mockPermissionRepository.Verify();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeAssignableTo<IReadOnlyList<Permission>>();
        result.Value.Should().HaveSameCount(sampleData);
    }
}
