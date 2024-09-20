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

    public async Task<IReadOnlyList<Role>> GetAllAsync() => await
            _Context
            .Roles
            .AsNoTracking()
            .ToListAsync();

    public async Task<Role?> GetByIdAsync(int id) => await _Context
            .Roles
            .FindAsync(id);

    public async Task<IReadOnlySet<string>> GetPermissionsByUserIdAsync(Guid userId)
    {
        Role[] roles = await _Context
            .Users
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id ==  userId)
            .Select(x => x.Role)
            .ToArrayAsync();
        return roles
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }

    public Task<bool> RoleExistsAsync(string roleName)
    {
        throw new NotImplementedException();
    }
}
