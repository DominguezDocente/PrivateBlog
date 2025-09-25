using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.Data.Seeders
{
    public class SectionsSeeder
    {
        private readonly DataContext _context;

        public SectionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Section> sections = new List<Section>() 
            {
                new Section { Id = Guid.NewGuid(), Name = "General", Description = "Información base de la compañia"},
                new Section { Id = Guid.NewGuid(), Name = "Informática"},
                new Section { Id = Guid.NewGuid(), Name = "Pentesting"},
                new Section { Id = Guid.NewGuid(), Name = "Clases", IsHidden = true}
            };

            foreach (Section section in sections) 
            {
                bool exists = await _context.Sections.AnyAsync(s => s.Name == section.Name);

                if (!exists)
                {
                    await _context.Sections.AddAsync(section);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
