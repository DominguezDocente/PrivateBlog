using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Domain.Entities.Blogs;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IBlogsRepository : IRepository<Blog>
    {
        Task<Blog?> GetByIdWithSectionAsync(Guid id, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<Blog> Items, int TotalCount)> GetPagedAsync(
            PaginationRequest pagination,
            string? nameFilter,
            Guid? sectionIdFilter,
            bool? isPublishedFilter,
            CancellationToken cancellationToken = default);
    }
}
