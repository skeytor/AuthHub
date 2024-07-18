using AuthHub.Domain.Entities;
using AuthHub.Persistence.Configurations.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthHub.Persistence.Configurations;

internal class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Configure properties
        builder.ToTable(TableName.Roles);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(70);

        // Configure relationship
        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId);

        // Configure an index for name property, it must be unique
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
