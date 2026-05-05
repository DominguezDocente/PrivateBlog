using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Configurations
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.HasIndex(r => r.Name)
                   .IsUnique();
        }
    }
}
