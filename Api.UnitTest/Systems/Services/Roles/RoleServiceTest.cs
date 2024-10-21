using AuthHub.Api.Dtos;
using AuthHub.Api.Services.Roles;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;
using FluentAssertions;
using Moq;

namespace Api.UnitTest.Systems.Services.Roles;

public class RoleServiceTest
{
    [Fact]
    public async Task CreateAsync_Should_ReturnRoleName_WhenRoleIsValid()
    {
        // Arrange
        CreateRoleRequest request = new("Admin", "This is an admin", []);
        Mock<IRoleRepository> mockRoleRepository = new();
        Mock<IPermissionRepository> mockPermissionRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();

        mockRoleRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<Role>()))
            .ReturnsAsync(new Role { Name = request.Name, Description = request.Description })
            .Verifiable(Times.Once());
        mockRoleRepository
            .Setup(repo => repo.RoleExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false)
            .Verifiable(Times.Once());
        mockPermissionRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync([new() { Id = 1, Name = "PermissionTest" }])
            .Verifiable(Times.Once());
        mockUnitOfWork
            .Setup(unit => unit.SaveChangesAsync(default))
            .ReturnsAsync(1)
            .Verifiable(Times.Once());

        RoleService service = new(
            mockRoleRepository.Object,
            mockPermissionRepository.Object,
            mockUnitOfWork.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Asseert
        mockRoleRepository.Verify();
        mockUnitOfWork.Verify();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();

        string roleName = result.Value;
        roleName.Should().Be(request.Name);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnValidationError_WhenRoleExists()
    {
        // Arrange
        CreateRoleRequest request = new("Admin", "This is an admin", []);
        Mock<IRoleRepository> mockRoleRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();

        mockRoleRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<Role>()))
            .ReturnsAsync(new Role())
            .Verifiable(Times.Never()); // Should never be called
        mockRoleRepository
            .Setup(repo => repo.RoleExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true)
            .Verifiable(Times.Once());
        mockUnitOfWork
            .Setup(unit => unit.SaveChangesAsync(default))
            .ReturnsAsync(0)
            .Verifiable(Times.Never()); // Should never be called to.

        RoleService service = new(mockRoleRepository.Object, default!, mockUnitOfWork.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Asseert
        mockRoleRepository.Verify();
        mockUnitOfWork.Verify();
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnRolesList_WhenRolesExist()
    {
        // Arrange
        IReadOnlyList<Role> roles =
        [
            new()
            {
                Name = "Admin",
                Description = "This is an admin user",
                Permissions = [new() { Name = "CanRead"}, new() { Name = "CanDelete"}]
            },
            new()
            {
                Name = "Supervisor",
                Description = "This is a supervisor user",
                Permissions = [new() { Name = "CanView"}]
            }
        ];
        Mock<IRoleRepository> mockRoleRepository = new();
        Mock<IPermissionRepository> mockPermissionRepository = new();
        mockRoleRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(roles)
            .Verifiable(Times.Once());
        mockPermissionRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync([new() { Id = 1, Name = "PermissionTest" }])
            .Verifiable(Times.Once());
        RoleService service = new(mockRoleRepository.Object, mockPermissionRepository.Object, default!);

        // Act
        var result = await service.GetAllAsync();

        // Asseert
        mockRoleRepository.Verify();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeAssignableTo<IReadOnlyList<RoleResponse>>();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().HaveSameCount(roles);
    }
}
