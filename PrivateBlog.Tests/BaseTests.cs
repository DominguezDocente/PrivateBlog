using Microsoft.EntityFrameworkCore;
using PrivateBlog.Persistence;

namespace PrivateBlog.Tests;

public abstract class BaseTests
{
    protected static DataContext BuildContext(string? databaseName = null)
    {
        string dbName = databaseName ?? Guid.NewGuid().ToString();

        DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        DataContext context = new DataContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    protected static async Task SaveChangesAsync(DataContext context)
    {
        await context.SaveChangesAsync();
    }
}
