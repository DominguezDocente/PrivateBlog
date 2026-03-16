using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Persistence.Configurations
{
    public class SectionConfig : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.Property(p => p.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(p => p.IsActive)
                   .IsRequired();
        }
    }
}
