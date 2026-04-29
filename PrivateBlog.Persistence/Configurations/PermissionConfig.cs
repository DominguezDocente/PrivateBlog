using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Domain.Entities.Users;

namespace PrivateBlog.Persistence.Configurations
{
    public sealed class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Code)
                   .HasMaxLength(120)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(300)
                   .IsRequired();

            builder.HasIndex(p => p.Code)
                   .IsUnique();
        }
    }
}
