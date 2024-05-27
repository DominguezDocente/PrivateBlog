using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PrivateBlogRole> PrivateBlogRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RoleSection> RoleSections { get; set; }
        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureIndexes(modelBuilder);
            ConfigureKeys(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Sections
            modelBuilder.Entity<Section>()
                        .HasIndex(s => s.Name)
                        .IsUnique();

            // Roles
            modelBuilder.Entity<PrivateBlogRole>()
                        .HasIndex(s => s.Name)
                        .IsUnique();
            // Users
            modelBuilder.Entity<User>()
                        .HasIndex(s => s.Document)
                        .IsUnique();
        }

        private void ConfigureKeys(ModelBuilder modelBuilder)
        {
            // Role Permission
            modelBuilder.Entity<RolePermission>()
                        .HasKey(rs => new { rs.RoleId, rs.PermissionId });

            modelBuilder.Entity<RolePermission>()
                        .HasOne(rs => rs.Role)
                        .WithMany(r => r.RolePermissions)
                        .HasForeignKey(rs => rs.RoleId);

            modelBuilder.Entity<RolePermission>()
                        .HasOne(rs => rs.Permission)
                        .WithMany(s => s.RolePermissions)
                        .HasForeignKey(rs => rs.PermissionId);

            // Role Section
            modelBuilder.Entity<RoleSection>()
                        .HasKey(rs => new { rs.RoleId, rs.SectionId });

            modelBuilder.Entity<RoleSection>()
                        .HasOne(rs => rs.Role)
                        .WithMany(r => r.RoleSections)
                        .HasForeignKey(rs => rs.RoleId);

            modelBuilder.Entity<RoleSection>()
                        .HasOne(rs => rs.Section)
                        .WithMany(s => s.RoleSections)
                        .HasForeignKey(rs => rs.SectionId);
        }
    }
}