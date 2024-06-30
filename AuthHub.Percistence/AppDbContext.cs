using AuthHub.Domain.Entities;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AuthHub.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
