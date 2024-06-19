using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AuthHub.Persistence.Repositories
{
    public sealed class RoleRepository(IAppDbContext context) 
        : BaseRepository(context), IRoleRepository
    {
        public async Task<IReadOnlyCollection<Role>> GetAllAsync()
        {
            return await
                _Context
                .Roles
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
