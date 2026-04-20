namespace PrivateBlog.Persistence.Seeding
{
    /// <summary>
    /// Orquesta todos los seeders por entidad (orden = orden del array).
    /// </summary>
    public static class SeedDb
    {
        private static readonly EntitySeederBase[] Seeders =
        [
            new SectionsSeeder(),
        ];

        public static async Task SeedAsync(DataContext db, CancellationToken cancellationToken = default)
        {
            foreach (EntitySeederBase seeder in Seeders)
            {
                await seeder.SeedAsync(db, cancellationToken);
            }
        }
    }
}
