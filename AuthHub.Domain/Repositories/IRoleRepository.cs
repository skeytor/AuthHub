using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<IReadOnlyList<Role>> GetAllAsync();
        Task<Role> InsertAsync(Role role);
        Task<Role?> GetByIdAsync(int id);
        Task<IReadOnlySet<string>> GetPermissionsByUserIdAsync(Guid userId);
        Task<bool> RoleExistsAsync(string roleName);
        Task UpdateAsync(Role role);
    }
}
