using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Persistence.Extensions;

namespace PrivateBlog.Persistence.Repositories
{
    public class SectionsRepository : Repository<Section>, ISectionsRepository
    {
        private readonly DataContext _context;

        public SectionsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Section> items, int totalCount)> GetPagedList(PaginationRequest request, 
                                                                        string? nameFilter, 
                                                                        bool? isActiveFilter, 
                                                                        CancellationToken cancellationToken = default)
        {
            IQueryable<Section> query = _context.Sections.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                string term = nameFilter.Trim();
                query = query.Where(s => s.Name.Contains(term));
            }

            if (isActiveFilter.HasValue)
            {
                query = query.Where(s => s.IsActive == isActiveFilter.Value);
            }

            (List<Section> items, int totalCount) = await query.OrderBy(s => s.Name)
                                                               .ToPagedListAsync(request, cancellationToken);

            return (items, totalCount);
        }
    }
}
