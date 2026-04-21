using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;

namespace PrivateBlog.Web.DTOs.Sections
{
    public sealed class SectionsIndexViewModel
    {
        public required PaginationResponse<SectionListItemDTO> List { get; set; }
        public string FilterName { get; set; } = string.Empty;
        public bool? FilterIsActive { get; set; }
    }
}
