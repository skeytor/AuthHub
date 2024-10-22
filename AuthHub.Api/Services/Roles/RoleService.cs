using AuthHub.Api.Dtos;
using AuthHub.Domain.Entities;
using AuthHub.Domain.Errors;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;
using AuthHub.Persistence.Abstractions;

namespace AuthHub.Api.Services.Roles;

public sealed class RoleService(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository,
    IUnitOfWork unitOfWork) : IRoleService
{
    public async Task<Result<string>> CreateAsync(CreateRoleRequest request)
    {
        if (await roleRepository.RoleExistsAsync(request.Name))
        {
            return Result.Failure<string>(RoleError.NotFoundByName(request.Name));
        }

        List<Permission> permissions = await permissionRepository.GetAllAsync();
        Permission[] rolePermissions = permissions
            .Where(x => request.Permissions.Contains(x.Id))
            .ToArray();
        
        if (rolePermissions.Length != request.Permissions.Length)
        {
            return Result.Failure<string>(RoleError.IdInvalid);
        }
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

    public async Task<Result<string>> UpdateAsync(int id, CreateRoleRequest request)
    {
        Role? role = await roleRepository.GetByIdAsync(id);
        if (role is null)
        {
            return Result.Failure<string>(Error.NotFound("", ""));
        }
        List<Permission> permissions = await permissionRepository.GetAllAsync();
        var rolePermissions = permissions
            .Where(x => request.Permissions.Contains(x.Id))
            .ToList();
        role.Name = request.Name;
        role.Description = request.Description;
        role.Permissions = rolePermissions;
        await Task.WhenAll(roleRepository.UpdateAsync(role), unitOfWork.SaveChangesAsync());
        return request.Name;
    }
}
