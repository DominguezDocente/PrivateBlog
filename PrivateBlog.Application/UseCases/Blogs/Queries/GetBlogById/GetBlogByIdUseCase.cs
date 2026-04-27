using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Blogs;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogById
{
    public sealed class GetBlogByIdUseCase : IRequestHandler<GetBlogByIdQuery, BlogDetailDTO?>
    {
        private readonly IBlogsRepository _blogsRepository;

        public GetBlogByIdUseCase(IBlogsRepository blogsRepository)
        {
            _blogsRepository = blogsRepository;
        }

        public async Task<BlogDetailDTO?> Handle(GetBlogByIdQuery request)
        {
            Blog? blog = await _blogsRepository.GetByIdWithSectionAsync(request.Id);

            if (blog is null)
            {
                return null;
            }

            return new BlogDetailDTO
            {
                Id = blog.Id,
                Name = blog.Name,
                Content = blog.Content,
                SectionId = blog.SectionId,
                SectionName = blog.Section.Name,
                IsPublished = blog.IsPublished,
                CreatedAtUtc = blog.CreatedAtUtc,
                UpdatedAtUtc = blog.UpdatedAtUtc,
            };
        }
    }
}
