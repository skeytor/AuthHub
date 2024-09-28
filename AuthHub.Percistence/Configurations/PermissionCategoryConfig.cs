using AuthHub.Domain.Entities;
using AuthHub.Persistence.Configurations.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthHub.Persistence.Configurations;

internal sealed class PermissionCategoryConfig : IEntityTypeConfiguration<PermissionCategory>
{
    public void Configure(EntityTypeBuilder<PermissionCategory> builder)
    {
        builder.ToTable(TableName.PermissionCategory);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(50);

        // Configure relationship
        builder.HasMany(x => x.Permissions)
            .WithOne()
            .HasForeignKey(x => x.PermissionCategoryId);
    }
}
