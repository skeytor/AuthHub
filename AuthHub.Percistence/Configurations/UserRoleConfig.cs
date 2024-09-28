using AuthHub.Domain.Entities;
using AuthHub.Persistence.Configurations.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthHub.Persistence.Configurations;

internal sealed class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(TableName.UserRole);
        builder.HasKey(x => new { x.RoleId, x.UserId });
    }
}
