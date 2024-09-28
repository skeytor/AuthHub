using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories;
public interface IUserRepository
{
    Task<User> InsertAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByUserNameAsync(string userName);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UserNameExistsAsync(string userName);
    Task<HashSet<string>> GetPermissions(Guid userId);
    void Update(User user);
}
