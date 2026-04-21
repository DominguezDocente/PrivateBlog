using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Seeding
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await new SectionsSeeder(_context).SeedAsync();
        }
    }
}
