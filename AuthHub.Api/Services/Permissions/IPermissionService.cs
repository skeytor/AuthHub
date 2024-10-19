using AuthHub.Domain.Entities;
using AuthHub.Domain.Results;

namespace AuthHub.Api.Services.Permissions;

public interface IPermissionService
{
    Task<Result<IReadOnlyList<Permission>>> GetAllAsync();
}
