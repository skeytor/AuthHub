using AuthHub.Api.Dtos;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.Roles;

public interface IRoleService
{
    Task<Result<string>> CreateAsync(CreateRoleRequest request);
    Task<Result<IReadOnlyList<RoleResponse>>> GetAllAsync();
}
