using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogById
{
    public sealed class GetBlogByIdQuery : IRequest<BlogDetailDTO?>
    {
        public Guid Id { get; init; }
    }
}
