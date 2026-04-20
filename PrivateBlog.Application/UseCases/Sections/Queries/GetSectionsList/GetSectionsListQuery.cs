using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList
{
    public class GetSectionsListQuery : IRequest<PaginationResponse<SectionListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = new();

        /// <summary>Filtro opcional: nombre contiene el texto (sin distinguir mayúsculas en base de datos típica).</summary>
        public string? NameFilter { get; set; }

        /// <summary>Filtro opcional: null = todas, true = solo activas, false = solo inactivas.</summary>
        public bool? IsActiveFilter { get; set; }
    }
}
