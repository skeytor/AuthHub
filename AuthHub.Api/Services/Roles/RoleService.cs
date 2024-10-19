using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Errors;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;

namespace AuthHub.Api.Services.Roles;

public sealed class RoleService(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IPermissionRepository? permissionRepository = null) : IRoleService
{
    public async Task<Result<string>> CreateAsync(CreateRoleRequest request)
    {
        if (await roleRepository.RoleExistsAsync(request.Name))
        {
            return Result.Failure<string>(RoleError.NotFoundByName(request.Name));
        }
        List<Permission> permissions = await permissionRepository!.GetAllAsync();

        Permission[] rolePermissions = permissions
            .Where(x => request.Permissions.Contains(x.Id))
            .ToArray();
        Role role = new()
        {
            Name = request.Name,
            Description = request.Description,
            Permissions = [.. rolePermissions]
        };
        Role roleCreated = await roleRepository.InsertAsync(role);
        await unitOfWork.SaveChangesAsync();
        return roleCreated.Name;
    }
    public async Task<Result<IReadOnlyList<RoleResponse>>> GetAllAsync()
    {
        IReadOnlyList<Role> roles = await roleRepository.GetAllAsync();
        return roles
            .Select(x => new RoleResponse(
                x.Id,
                x.Name,
                x.Description,
                x.Permissions.Select(p => p.Name).ToHashSet()))
            .ToList();
    }
}
