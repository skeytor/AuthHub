using AuthHub.Domain.Entities;
using AuthHub.Domain.Repositories;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AuthHub.Persistence.Repositories;

public sealed class PermissionRepository(IAppDbContext context)
    : BaseRepository(context), IPermissionRepository
{
    public async Task<List<Permission>> GetAllAsync() => await _Context
            .Permissions
            .ToListAsync();
}
