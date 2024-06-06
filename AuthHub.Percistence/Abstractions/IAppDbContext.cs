using AuthHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthHub.Persistence.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
    }
}
