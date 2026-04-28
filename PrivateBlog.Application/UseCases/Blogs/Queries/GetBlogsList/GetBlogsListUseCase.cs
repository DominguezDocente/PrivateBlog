using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

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
            PaginationRequest pagination = request.Pagination;

            (IReadOnlyList<Blog> blogs, int totalCount) =
                await _blogsRepository.GetPagedListAsync(
                    pagination,
                    request.NameFilter,
                    request.SectionIdFilter,
                    request.IsPublishedFilter);

            List<BlogListItemDTO> items = blogs.Select(b => b.ToListItemDTO()).ToList();

            return PaginationResponse<BlogListItemDTO>.Create(items, totalCount, pagination);
        }
    }
}
