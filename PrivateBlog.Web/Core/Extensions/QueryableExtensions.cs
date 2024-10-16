using PrivateBlog.Web.Core.Pagination;

namespace PrivateBlog.Web.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> querable, PaginationRequest request)
        {
            return querable.Skip((request.Page - 1) * request.RecordsPerPage)
                           .Take(request.RecordsPerPage);
        }
    }
}
