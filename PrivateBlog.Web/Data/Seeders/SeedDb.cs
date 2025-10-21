using PrivateBlog.Web.Services.Abtractions;

namespace PrivateBlog.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public SeedDb(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await new SectionsSeeder(_context).SeedAsync();
            await new BlogsSeeder(_context).SeedAsync();
            await new PermissionsSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _usersService).SeedAsync();
        }
    }
}
