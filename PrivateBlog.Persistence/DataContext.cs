using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Entities.Users;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected DataContext()
        {
        }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
