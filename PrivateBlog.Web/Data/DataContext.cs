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

        public DbSet<Section> Sections { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<PrivateBlogRole> PrivateBlogRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureKeys(builder);
            ConfigureIdexes(builder);

            base.OnModelCreating(builder);
        }

        private void ConfigureKeys(ModelBuilder builder)
        {
            // Role Permission
            builder.Entity<RolePermission>().HasKey(rp => new { rp.PrivateBlogRoleId, rp.PermissionId });

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                            .WithMany(p => p.RolePermissions)
                                            .HasForeignKey(rp => rp.PermissionId);

            builder.Entity<RolePermission>().HasOne(rp => rp.PrivateBlogRole)
                                            .WithMany(r => r.RolePermissions)
                                            .HasForeignKey(rp => rp.PrivateBlogRoleId);
        }

        private void ConfigureIdexes(ModelBuilder builder)
        {
            // Roles
            builder.Entity<PrivateBlogRole>().HasIndex(r => r.Name)
                                             .IsUnique();

            // Sections
            builder.Entity<Section>().HasIndex(s => s.Name)
                                     .IsUnique();

            // User
            builder.Entity<User>().HasIndex(u => u.Document)
                                  .IsUnique();

            // Permission
            builder.Entity<Permission>().HasIndex(p => p.Code)
                                        .IsUnique();
        }
    }
}
