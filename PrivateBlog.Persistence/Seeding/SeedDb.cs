using Microsoft.AspNetCore.Identity;
using PrivateBlog.Persistence.Entitities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Seeding
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeedDb(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await new SectionsSeeder(_context).SeedAsync();
            await new UsersSeeder(_userManager).SeedAsync();
        }
    }
}
