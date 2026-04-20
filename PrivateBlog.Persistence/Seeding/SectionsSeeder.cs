using Microsoft.EntityFrameworkCore;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Persistence.Seeding
{
    public sealed class SectionsSeeder : EntitySeederBase
    {
        private static readonly string[] SeedNames =
        [
            "General",
            "Noticias",
            "Tutoriales",
            "Recursos",
        ];

        public override async Task SeedAsync(DataContext db, CancellationToken cancellationToken = default)
        {
            List<string> existingNames = await db.Sections
                .Select(s => s.Name)
                .ToListAsync(cancellationToken);

            HashSet<string> existing = new(existingNames, StringComparer.OrdinalIgnoreCase);

            List<Section> toAdd = [];

            foreach (string name in SeedNames)
            {
                if (existing.Contains(name))
                {
                    continue;
                }

                toAdd.Add(new Section(name));
                existing.Add(name);
            }

            if (toAdd.Count == 0)
            {
                return;
            }

            db.Sections.AddRange(toAdd);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
