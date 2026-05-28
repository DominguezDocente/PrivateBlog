using Microsoft.EntityFrameworkCore;
using PrivateBlog.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Tests
{
    public class BaseTests
    {
        protected static DataContext BuildContext(string? dbName = null)
        {
            dbName = dbName ?? Guid.NewGuid().ToString();

            DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            DataContext context = new DataContext(options);

            return context;
        }

        protected static async Task SaveChangesAsync(DataContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}
