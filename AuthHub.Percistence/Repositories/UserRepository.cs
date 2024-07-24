using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthHub.Persistence.Repositories;
public sealed class UserRepository(IAppDbContext context)
    : BaseRepository(context), IUserRepository
{
    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> userCreated = await _Context
            .Users
            .AddAsync(user);
        return userCreated.Entity;
    }

    public Task<bool> ExistAsync(string email)
    {
        return _Context
            .Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _Context
            .Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await
            _Context
            .Users
            .FindAsync(id);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await
            _Context
            .Users
            .FindAsync(userName);
    }
}
