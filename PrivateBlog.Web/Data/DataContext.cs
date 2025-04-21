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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureKeys(builder);
            ConfigureIndexes(builder);

            base.OnModelCreating(builder);
        }

        private void ConfigureKeys(ModelBuilder builder)
        {
            // Role Permissions
            builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>().HasOne(rp => rp.Role)
                                            .WithMany(r => r.RolePermissions)
                                            .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                            .WithMany(p => p.RolePermissions)
                                            .HasForeignKey(rp => rp.PermissionId);

            // Role Sections
            builder.Entity<RoleSection>().HasKey(rs => new { rs.RoleId, rs.SectionId });

            builder.Entity<RoleSection>().HasOne(rs => rs.Role)
                                         .WithMany(r => r.RoleSections)
                                         .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RoleSection>().HasOne(rs => rs.Section)
                                         .WithMany(p => p.RoleSections)
                                         .HasForeignKey(rs => rs.SectionId);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            // Roles
            builder.Entity<PrivateBlogRole>().HasIndex(r => r.Name)
                                             .IsUnique();
            // Sections
            builder.Entity<Section>().HasIndex(s => s.Name)
                                     .IsUnique();
            // Users
            builder.Entity<User>().HasIndex(u => u.Document)
                                  .IsUnique();
        }
    }
}
