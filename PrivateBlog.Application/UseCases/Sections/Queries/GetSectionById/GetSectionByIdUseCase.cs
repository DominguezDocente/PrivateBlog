using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById
{
    public class GetSectionByIdUseCase : IRequestHandler<GetSectionByIdQuery, SectionDetailDTO>
    {
        private readonly ISectionsRepository _sectionsRepository;

        public GetSectionByIdUseCase(ISectionsRepository sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
        }

        public async Task<SectionDetailDTO> Handle(GetSectionByIdQuery request)
        {
            Section? section = await _sectionsRepository.GetByIdAsync(request.Id);

            if (section == null) 
            {
                throw new BussinesRuleException("La sección no existe");
            }

            return new SectionDetailDTO
            {
                Id = section.Id,
                Name = section.Name,
                IsActive = section.IsActive
            };
        }
    }
}
