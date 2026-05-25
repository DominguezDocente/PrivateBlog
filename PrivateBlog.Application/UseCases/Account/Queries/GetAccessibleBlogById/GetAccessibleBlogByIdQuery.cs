using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogById
{
    public class GetAccessibleBlogByIdQuery : IRequest<AccessibleBlogDetailDTO>
    {
        public required string UserId { get; set; }
        public required Guid BlogId { get; set; }
    }
}
