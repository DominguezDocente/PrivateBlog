using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Domain.Entities.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IBlogsRepository : IRepository<Blog>
    {
        Task<Blog?> GetByIdWithSectionAsync(Guid id, CancellationToken cancellationToken = default);

        Task<(List<Blog> items, int totalCount)> GetPagedListAsync(PaginationRequest request,
                                                                 string? filter,
                                                                 Guid? sectionIdFilter,
                                                                 bool? isPublishedFilter,
                                                                 CancellationToken cancellationToken = default);
    }
}
