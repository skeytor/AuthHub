using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthHub.Persistence.Repositories;
public sealed class UserRepository(IAppDbContext context)
    : BaseRepository(context), IUserRepository
{
    public async Task<User> InsertAsync(User user)
    {
        EntityEntry<User> userCreated = await _Context
            .Users
            .AddAsync(user);
        return userCreated.Entity;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync() => await _Context
            .Users
            .AsNoTracking()
            .ToListAsync();

    public async Task<User?> GetByIdAsync(Guid id) => await _Context
            .Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetByUserNameAsync(string userName) => await _Context
            .Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(u => u.Username == userName);

    public async Task<bool> EmailExistsAsync(string email) => await _Context
            .Users
            .AnyAsync(u => u.Email == email);

    public async Task<bool> UserNameExistsAsync(string userName) => await _Context
            .Users
            .AnyAsync(u => u.Username == userName);

    public void Update(User user) => _Context.Users.Update(user);

    public async Task<HashSet<string>> GetPermissionByUserIdAsync(Guid userId)
    {
        string[] permission = await _Context
            .Users
            .AsNoTracking()
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Role)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToArrayAsync();
        return [.. permission];
    }
}
