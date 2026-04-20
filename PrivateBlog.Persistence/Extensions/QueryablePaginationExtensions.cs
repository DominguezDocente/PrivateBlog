using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Pagination;

namespace PrivateBlog.Persistence.Extensions
{
    /// <summary>
    /// Reutilizar en otros repositorios: ordenar antes con OrderBy / OrderByDescending y luego ToPagedListAsync.
    /// </summary>
    internal static class QueryablePaginationExtensions
    {
        public static async Task<(List<T> Items, int TotalCount)> ToPagedListAsync<T>(
            this IOrderedQueryable<T> orderedQuery,
            PaginationRequest pagination,
            CancellationToken cancellationToken = default)
        {
            PaginationRequest p = pagination.Normalized();

            int totalCount = await orderedQuery.CountAsync(cancellationToken);

            List<T> items = await orderedQuery
                .Skip((p.PageNumber - 1) * p.PageSize)
                .Take(p.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
