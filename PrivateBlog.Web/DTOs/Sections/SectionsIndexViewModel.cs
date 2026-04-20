using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;

namespace PrivateBlog.Web.DTOs.Sections
{
    /// <summary>
    /// Vista del listado de secciones: resultado paginado + estado del filtro (sin ViewData).
    /// </summary>
    public sealed class SectionsIndexViewModel
    {
        public required PaginationResponse<SectionListItemDTO> List { get; init; }

        public string FilterName { get; init; } = string.Empty;

        public bool? FilterIsActive { get; init; }
    }
}
