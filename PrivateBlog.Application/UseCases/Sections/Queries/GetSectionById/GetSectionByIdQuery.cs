using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById
{
    public class GetSectionByIdQuery : IRequest<SectionDetailDTO>
    {
        public required Guid Id { get; set; }
    }
}
