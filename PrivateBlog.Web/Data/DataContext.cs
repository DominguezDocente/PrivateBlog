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
    }
}
