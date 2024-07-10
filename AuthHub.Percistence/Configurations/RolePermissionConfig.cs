using AuthHub.Domain.Entities;
using AuthHub.Persistence.Configurations.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthHub.Persistence.Configurations;

internal sealed class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(TableName.RolePermissions);
        builder.HasKey(x => new { x.RoleId, x.PermissionId });
    }
}
