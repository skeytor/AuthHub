using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthHub.Persistence.Repositories;

public sealed class RoleRepository(IAppDbContext context) 
    : BaseRepository(context), IRoleRepository
{
    public async Task<Role> InsertAsync(Role role)
    {
        EntityEntry<Role> roleCreated = await _Context.Roles.AddAsync(role);
        return roleCreated.Entity;
    }

    public async Task<IReadOnlyList<Role>> GetAllAsync() => await _Context
            .Roles
            .AsNoTracking()
            .Include(x => x.Permissions)
            .ToListAsync();

    public async Task<Role?> GetByIdAsync(int id) => await _Context
            .Roles
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlySet<string>> GetPermissionsByUserIdAsync(Guid userId)
    {
        string[] roles = await _Context
            .Users
            .AsNoTracking()
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Role)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToArrayAsync();
        return roles.ToHashSet();
    }

    public async Task<bool> RoleExistsAsync(string roleName) => await _Context
            .Roles
            .AnyAsync(x => x.Name == roleName);

    public Task UpdateAsync(Role role) => Task.FromResult(_Context.Roles.Update(role));
}
