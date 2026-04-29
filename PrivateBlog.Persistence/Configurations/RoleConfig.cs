using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Domain.Entities.Users;

namespace PrivateBlog.Persistence.Configurations
{
    public sealed class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.HasIndex(r => r.Name)
                   .IsUnique();
        }
    }
}
