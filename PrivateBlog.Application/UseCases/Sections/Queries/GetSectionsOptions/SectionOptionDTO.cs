using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions
{
    public sealed class SectionOptionDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = null!;
    }
}
