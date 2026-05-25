using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleSections
{
    public class GetAccessibleSectionsQuery : IRequest<IReadOnlyList<AccessibleSectionItemDTO>>
    {
        public required string UserId { get; set; }
    }
}
