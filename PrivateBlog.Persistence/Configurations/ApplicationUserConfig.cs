using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(u => u.LastName)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(u => u.RoleId)
                   .IsRequired();

            builder.HasOne(u => u.Role)
                   .WithMany()
                   .HasForeignKey(u => u.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}