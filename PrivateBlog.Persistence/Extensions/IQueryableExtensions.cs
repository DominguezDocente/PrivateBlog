namespace PrivateBlog.Persistence.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int recordsPerPage)
        {
            int skip = (page - 1) * recordsPerPage;
            return query.Skip(skip).Take(recordsPerPage);
        }
    }
}
