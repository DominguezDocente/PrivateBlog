using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionOptions
{
    public sealed class GetSectionOptionsUseCase : IRequestHandler<GetSectionOptionsQuery, IReadOnlyList<SectionOptionDTO>>
    {
        private readonly ISectionsRepository _sectionsRepository;

        public GetSectionOptionsUseCase(ISectionsRepository sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
        }

        public async Task<IReadOnlyList<SectionOptionDTO>> Handle(GetSectionOptionsQuery request)
        {
            IEnumerable<Section> sections = await _sectionsRepository.GetListAsync();

            return sections
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new SectionOptionDTO { Id = s.Id, Name = s.Name })
                .ToList();
        }
    }
}
