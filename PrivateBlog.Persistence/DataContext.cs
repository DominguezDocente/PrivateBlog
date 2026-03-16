using Microsoft.EntityFrameworkCore;
using PrivateBlog.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected DataContext() { }

        public DbSet<Section> Sections { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
