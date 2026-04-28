using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions
{
    public sealed class GetSectionOptionsQuery : IRequest<IReadOnlyList<SectionOptionDTO>>
    {
    }
}
