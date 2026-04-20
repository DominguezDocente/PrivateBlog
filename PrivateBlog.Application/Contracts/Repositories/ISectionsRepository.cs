using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface ISectionsRepository : IRepository<Section>
    {
        Task<(IReadOnlyList<Section> Items, int TotalCount)> GetPagedByNameAsync(
            PaginationRequest pagination,
            string? nameFilter,
            bool? isActiveFilter,
            CancellationToken cancellationToken = default);
    }
}
