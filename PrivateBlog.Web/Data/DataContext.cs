using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureIndexes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Sections
            modelBuilder.Entity<Section>()
                        .HasIndex(s => s.Name)
                        .IsUnique();
        }
    }
}