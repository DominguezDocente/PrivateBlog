using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence.Seeding
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public SeedDb(
            DataContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            await new SectionsSeeder(_context).SeedAsync();
            await new PermissionsSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _userManager, _configuration).SeedAsync();
        }
    }
}
