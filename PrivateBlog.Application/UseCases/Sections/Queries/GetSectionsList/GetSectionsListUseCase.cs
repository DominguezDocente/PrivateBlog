using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList
{
    public class GetSectionsListUseCase : IRequestHandler<GetSectionsListQuery, IEnumerable<SectionListItemDTO>>
    {
        private readonly ISectionsRepository _sectionsRepository;

        public GetSectionsListUseCase(ISectionsRepository sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
        }

        public async Task<IEnumerable<SectionListItemDTO>> Handle(GetSectionsListQuery request)
        {
            IEnumerable<Section> sections = await _sectionsRepository.GetListAsync();
            List<SectionListItemDTO> sectionsDTO = sections
                .OrderBy(s => s.Name)
                .Select(c => c.ToDTO())
                .ToList();

            return sectionsDTO;
        }
    }
}
