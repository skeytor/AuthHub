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

    public async Task<User?> GetByIdAsync(Guid id) => await
            _Context
            .Users
            .FindAsync(id);

    public async Task<User?> GetByUserNameAsync(string userName) => await
            _Context
            .Users
            .FindAsync(userName);

    public Task<bool> IsUniqueByEmailAsync(string email) => _Context
            .Users
            .AnyAsync(u => u.Email == email);

    public Task<bool> IsUniqueByUserNameAsync(string userName) => _Context
            .Users
            .AnyAsync(u => u.Username == userName);

    public void Update(User user) => _Context.Users.Update(user);
}
