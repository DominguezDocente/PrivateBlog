using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.DeleteBlog
{
    public sealed class DeleteBlogCommand : IRequest
    {
        public Guid Id { get; init; }
    }
}
