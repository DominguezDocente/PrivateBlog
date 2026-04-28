using Microsoft.EntityFrameworkCore;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
