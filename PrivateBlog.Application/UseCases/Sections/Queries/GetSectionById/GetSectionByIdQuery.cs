using PrivateBlog.Application.Utils.Mediator;
using System;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById
{
    public class GetSectionByIdQuery : IRequest<SectionDetailDTO?>
    {
        public required Guid Id { get; set; }
    }
}
