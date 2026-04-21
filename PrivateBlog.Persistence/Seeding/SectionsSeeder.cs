using PrivateBlog.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Seeding
{
    internal class SectionsSeeder : ISeedable
    {
        private readonly DataContext _context;

        public SectionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            string[] sections =
            [
                "Technology",
                "Lifestyle",
                "Travel",
                "Food",
                "Health",
                "Education",
                "Entertainment",
                "Business",
                "Sports",
                "Politics"
            ];

            foreach (string section in sections) 
            {
                if (!_context.Sections.Any(s => s.Name == section))
                {
                    await _context.Sections.AddAsync(new Section(section));
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
