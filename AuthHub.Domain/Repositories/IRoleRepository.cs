using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<IReadOnlyList<Role>> GetAllAsync();
        Task<Role> CreateAsync(Role role);
        Task<Role> GetByIdAsync(int id);
        Task<IReadOnlyList<HashSet<Permission>>> GetPermissionsAsync(int roleId);
    }
}
