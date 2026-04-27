using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Persistence.Extensions;

namespace PrivateBlog.Persistence.Repositories
{
    public class BlogsRepository : Repository<Blog>, IBlogsRepository
    {
        private readonly DataContext _db;

        public BlogsRepository(DataContext context)
            : base(context)
        {
            _db = context;
        }

        public async Task<Blog?> GetByIdWithSectionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Set<Blog>()
                .Include(b => b.Section)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<(IReadOnlyList<Blog> Items, int TotalCount)> GetPagedAsync(
            PaginationRequest pagination,
            string? nameFilter,
            Guid? sectionIdFilter,
            bool? isPublishedFilter,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Blog> query = _db.Set<Blog>().Include(b => b.Section).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                string term = nameFilter.Trim();
                query = query.Where(b => b.Name.Contains(term));
            }

            if (sectionIdFilter.HasValue)
            {
                Guid sid = sectionIdFilter.Value;
                query = query.Where(b => b.SectionId == sid);
            }

            if (isPublishedFilter.HasValue)
            {
                bool pub = isPublishedFilter.Value;
                query = query.Where(b => b.IsPublished == pub);
            }

            IOrderedQueryable<Blog> ordered = query.OrderByDescending(b => b.UpdatedAtUtc);

            (List<Blog> items, int totalCount) = await ordered.ToPagedListAsync(pagination, cancellationToken);

            return (items, totalCount);
        }
    }
}
