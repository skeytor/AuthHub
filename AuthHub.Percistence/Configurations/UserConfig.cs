using AuthHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthHub.Persistence.Configurations
{
    /// <summary>
    /// Configuration of User entity properties for store data in the database
    /// </summary>
    internal sealed class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Configure properties
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(70);
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(70);
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Configure relationship

            // Configure an index for username and email properties. They such be uniques
            builder.HasIndex(x => x.Username)
                .IsUnique();
            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
