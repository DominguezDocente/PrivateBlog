using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.CreateBlog
{
    public sealed class CreateBlogCommand : IRequest<Guid>
    {
        public required string Name { get; init; }

        public required string Content { get; init; }

        public Guid SectionId { get; init; }

        public bool IsPublished { get; init; }
    }
}
