using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface ISectionsRepository : IRepository<Section>
    {
        Task<(List<Section> items, int totalCount)> GetPagedList(PaginationRequest request, 
                                                                 string? nameFilter, 
                                                                 bool? isActiveFilter,
                                                                 CancellationToken cancellationToken = default);
    }
}
