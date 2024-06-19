using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<IReadOnlyCollection<Role>> GetAllAsync();
    }
}
