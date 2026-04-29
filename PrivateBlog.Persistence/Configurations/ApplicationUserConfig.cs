using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence.Configurations
{
    public sealed class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.RoleId)
                   .IsRequired();

            builder.HasOne(u => u.Role)
                   .WithMany()
                   .HasForeignKey(u => u.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => u.RoleId);

            builder.Property(u => u.FirstName)
                   .HasMaxLength(120);

            builder.Property(u => u.LastName)
                   .HasMaxLength(120);
        }
    }
}
