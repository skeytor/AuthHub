using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.Permissions;

public sealed class PermissionService(IPermissionRepository permissionRepository) 
    : IPermissionService
{
    public async Task<Result<IReadOnlyList<Permission>>> GetAllAsync()
    {
        IReadOnlyList<Permission> permission = await permissionRepository.GetAllAsync();
        return Result.Success(permission);
    }
}
