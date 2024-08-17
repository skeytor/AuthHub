using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories;
public interface IUserRepository
{
    Task<User> InsertAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByUserNameAsync(string userName);
    Task<bool> IsUniqueByEmailAsync(string email);
    Task<bool> IsUniqueByUserNameAsync(string userName);
    void Update(User user);
}
