using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList
{
    public static class MapperExtensions
    {
        public static SectionListItemDTO ToDTO(this Section section)
        {
            return new SectionListItemDTO
            {
                Id = section.Id,
                Name = section.Name,
            };
        }
    }
}
