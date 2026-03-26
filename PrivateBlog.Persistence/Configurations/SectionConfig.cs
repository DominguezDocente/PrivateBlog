using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Persistence.Configurations
{
    public class SectionConfig : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.Property(s => s.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(s => s.IsActive)
                   .IsRequired();
        }
    }
}
