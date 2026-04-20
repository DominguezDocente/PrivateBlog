using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Persistence.Extensions;

namespace PrivateBlog.Persistence.Repositories
{
    public class SectionsRepository : Repository<Section>, ISectionsRepository
    {
        private readonly DataContext _db;

        public SectionsRepository(DataContext context) : base(context)
        {
            _db = context;
        }

        public async Task<(IReadOnlyList<Section> Items, int TotalCount)> GetPagedByNameAsync(
            PaginationRequest pagination,
            string? nameFilter,
            bool? isActiveFilter,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Section> query = _db.Set<Section>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                string term = nameFilter.Trim();
                query = query.Where(s => s.Name.Contains(term));
            }

            if (isActiveFilter.HasValue)
            {
                bool active = isActiveFilter.Value;
                query = query.Where(s => s.IsActive == active);
            }

            IOrderedQueryable<Section> ordered = query.OrderBy(s => s.Name);

            (List<Section> items, int totalCount) = await ordered.ToPagedListAsync(pagination, cancellationToken);

            return (items, totalCount);
        }
    }
}
