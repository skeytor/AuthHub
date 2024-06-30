using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories;
public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<User> UpdateAsync(Guid id, User user);
    Task<IReadOnlyCollection<User>> GetAllAsync();
}
