namespace PrivateBlog.Persistence.Seeding
{
    /// <summary>
    /// Punto común para seeders por entidad (idempotentes donde aplique).
    /// </summary>
    public abstract class EntitySeederBase
    {
        public abstract Task SeedAsync(DataContext db, CancellationToken cancellationToken = default);
    }
}
