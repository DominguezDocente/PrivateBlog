using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateBlog.Domain.Entities.Blogs;

namespace PrivateBlog.Persistence.Configurations
{
    public class BlogConfig : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(b => b.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(b => b.Content)
                   .IsRequired();

            builder.Property(b => b.CreatedAtUtc)
                   .IsRequired();

            builder.Property(b => b.UpdatedAtUtc)
                   .IsRequired();

            builder.Property(b => b.IsPublished)
                   .IsRequired();

            builder.HasOne(b => b.Section)
                   .WithMany(s => s.Blogs)
                   .HasForeignKey(b => b.SectionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
