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
    }
}
