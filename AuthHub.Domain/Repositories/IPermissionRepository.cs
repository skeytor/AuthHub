using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories;

public interface IPermissionRepository
{
    Task<List<Permission>> GetAllAsync();
}
