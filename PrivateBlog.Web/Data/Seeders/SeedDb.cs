using PrivateBlog.Web.Services.Abstractions;

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
            await new AccountSeeder(_usersService, _context).SeedAsync();
        }
    }
}
