using PrivateBlog.Web.Core.Pagination;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class SectionDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        public string Name { get; set; } = null!;


        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "¿Está oculta?")]
        public bool IsHidden { get; set; } = false;

        public PaginationResponse<BlogDTO>? PaginatedBlogs { get; set; }
    }
}
