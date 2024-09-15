using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthHub.Persistence.Repositories;

public sealed class RoleRepository(IAppDbContext context) 
    : BaseRepository(context), IRoleRepository
{
    public async Task<Role> CreateAsync(Role role)
    {
        EntityEntry<Role> roleCreated = await _Context.Roles.AddAsync(role);
        return roleCreated.Entity;
    }

    public async Task<IReadOnlyList<Role>> GetAllAsync()
    {
        return await
            _Context
            .Roles
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Role> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<HashSet<Permission>>> GetPermissionsAsync(int roleId)
    {
        throw new NotImplementedException();
    }
}
