using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Security;
using PrivateBlog.Domain.Entities.Account;

namespace PrivateBlog.Persistence.Seeding
{
    internal sealed class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            foreach (PermissionCodesCatalog.PermissionSeed seed in PermissionCodesCatalog.All)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Code == seed.Code);
                if (!exists)
                {
                    await _context.Permissions.AddAsync(new Permission(seed.Code, seed.Description, seed.Module));
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
