using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogsBySection
{
    public class GetAccessibleBlogsBySectionQuery : IRequest<AccessibleSectionBlogsDTO>
    {
        public required string UserId { get; set; }
        public required Guid SectionId { get; set; }
    }
}
