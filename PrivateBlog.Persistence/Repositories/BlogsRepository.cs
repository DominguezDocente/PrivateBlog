using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Repositories
{
    public class BlogsRepository : Repository<Blog>, IBlogsRepository
    {
        private readonly DataContext _context;

        public BlogsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Blog?> GetByIdWithSectionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Blog>()
                                 .Include(b => b.Section)
                                 .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<(List<Blog> items, int totalCount)> GetPagedListAsync(PaginationRequest request, string? filter, Guid? sectionIdFilter, bool? isPublishedFilter, CancellationToken cancellationToken = default)
        {
            IQueryable<Blog> query = _context.Set<Blog>().Include(b => b.Section).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string term = filter.Trim();
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

            query = query.OrderByDescending(b => b.CreatedAt);

            (List<Blog> items, int totalCount) = await query.ToPagedListAsync(request, cancellationToken);

            return (items, totalCount);
        }
    }
}
