using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
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

        public async Task<PaginationResponse<SectionListItemDTO>> Handle(GetSectionsListQuery request)
        {
            PaginationRequest pagination = request.Pagination.Normalized();

            (IReadOnlyList<Section> sections, int totalCount) =
                await _sectionsRepository.GetPagedByNameAsync(
                    pagination,
                    request.NameFilter,
                    request.IsActiveFilter);

            List<SectionListItemDTO> items = sections.Select(s => s.ToDTO()).ToList();

            return PaginationResponse<SectionListItemDTO>.Create(items, totalCount, pagination);
        }
    }
}
