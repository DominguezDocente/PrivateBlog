using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;
        private readonly IAccountService _accountService;

        public SeedDb(DataContext context, IUsersService usersService, IAccountService accountService)
        {
            _context = context;
            _usersService = usersService;
            _accountService = accountService;
        }

        public async Task SeedAsync()
        {
            await new SectionsSeeder(_context).SeedAsync();
            await new PermissionsSeeder(_context).SeedAsync();
            await new AccountSeeder(_usersService, _context, _accountService).SeedAsync();
        }
    }
}
