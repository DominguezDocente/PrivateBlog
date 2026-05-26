using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Configurations
{
    internal class RoleSectionConfig : IEntityTypeConfiguration<RoleSection>
    {
        public void Configure(EntityTypeBuilder<RoleSection> builder)
        {
            builder.HasKey(rs => new { rs.RoleId, rs.SectionId });

            builder.HasOne(rs => rs.Role)
                   .WithMany(r => r.RoleSections)
                   .HasForeignKey(rs => rs.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rs => rs.Section)
                   .WithMany(s => s.RoleSections)
                   .HasForeignKey(rs => rs.SectionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
