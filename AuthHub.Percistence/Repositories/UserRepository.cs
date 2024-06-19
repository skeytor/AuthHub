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

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await
            _Context
            .Users
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await
            _Context
            .Users
            .FindAsync(id);
    }

    public Task<User> UpdateAsync(Guid id, User user)
    {
        throw new NotImplementedException();
    }
}
