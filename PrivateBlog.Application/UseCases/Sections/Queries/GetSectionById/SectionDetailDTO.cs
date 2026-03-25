using System;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById
{
    public class SectionDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
