using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionOptions
{
    public sealed class GetSectionOptionsQuery : IRequest<IReadOnlyList<SectionOptionDTO>>
    {
    }
}
