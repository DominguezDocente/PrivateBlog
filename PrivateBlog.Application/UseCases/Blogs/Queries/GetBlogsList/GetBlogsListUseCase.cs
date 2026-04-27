using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Blogs;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList
{
    public sealed class GetBlogsListUseCase : IRequestHandler<GetBlogsListQuery, PaginationResponse<BlogListItemDTO>>
    {
        private readonly IBlogsRepository _blogsRepository;

        public GetBlogsListUseCase(IBlogsRepository blogsRepository)
        {
            _blogsRepository = blogsRepository;
        }

        public async Task<PaginationResponse<BlogListItemDTO>> Handle(GetBlogsListQuery request)
        {
            PaginationRequest pagination = request.Pagination.Normalized();

            (IReadOnlyList<Blog> blogs, int totalCount) =
                await _blogsRepository.GetPagedAsync(
                    pagination,
                    request.NameFilter,
                    request.SectionIdFilter,
                    request.IsPublishedFilter);

            List<BlogListItemDTO> items = blogs.Select(b => b.ToListItemDTO()).ToList();

            return PaginationResponse<BlogListItemDTO>.Create(items, totalCount, pagination);
        }
    }
}
