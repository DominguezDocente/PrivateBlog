using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList
{
    public class GetSectionsListUseCase : IRequestHandler<GetSectionsListQuery, PaginationResponse<SectionListItemDTO>>
    {
        private readonly ISectionsRepository _sectionsRepository;

        public GetSectionsListUseCase(ISectionsRepository sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
        }

        public async Task<PaginationResponse<SectionListItemDTO>> Handle(GetSectionsListQuery query)
        {
            (List<Section> sections, int totalCount) = await _sectionsRepository.GetPagedList(query.Pagination, query.NameFilter, query.IsActiveFilter);

            List<SectionListItemDTO> sectionsDTO = sections.Select(s => s.ToDTO())
                                                           .ToList();

            return PaginationResponse<SectionListItemDTO>.Create(sectionsDTO, totalCount, query.Pagination);
        }
    }
}
