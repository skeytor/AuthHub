using AuthHub.Domain.Entities;
using AuthHub.Infrastructure.Authorization;
using AuthHub.Persistence.Configurations.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthHub.Persistence.Configurations;

internal sealed class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableName.Permissions);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        // Configure an index for name property, it must be unique
        builder.HasIndex(x => x.Name)
            .IsUnique();
        builder.HasData(GetPermissions());
    }

    private static IEnumerable<Permission> GetPermissions() => Enum
            .GetValues<Permissions>()
            .Where(x => x != Permissions.None && x != Permissions.All)
            .Select((p, i) => new Permission
            {
                Id = (int)p,
                Name = p.ToString()
            });
}
