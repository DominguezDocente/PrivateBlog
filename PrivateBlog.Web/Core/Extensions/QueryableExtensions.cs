using PrivateBlog.Web.Core.Pagination;

namespace PrivateBlog.Web.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PaginteAsync<T>(this IQueryable<T> queryable, PaginationRequest request)
        {
            return queryable.Skip((request.Page - 1) * request.RecordsPerPage)
                            .Take(request.RecordsPerPage);
        }
    }
}
