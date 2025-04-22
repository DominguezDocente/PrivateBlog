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
            ConfugureIndexes(builder);

            base.OnModelCreating(builder);
        }

        private void ConfugureIndexes(ModelBuilder builder)
        {
            // Roles
            builder.Entity<PrivateBlogRole>().HasIndex(r => r.Name)
                                             .IsUnique();
            // Sections
            builder.Entity<Section>().HasIndex(r => r.Name)
                                     .IsUnique();
            // Roles
            builder.Entity<User>().HasIndex(r => r.Document)
                                  .IsUnique();
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
                                         .HasForeignKey(rs => rs.RoleId);

            builder.Entity<RoleSection>().HasOne(rs => rs.Section)
                                            .WithMany(s => s.RoleSections)
                                            .HasForeignKey(rs => rs.SectionId);
        }
    }
}
