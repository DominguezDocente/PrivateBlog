using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Blog
{
    public class CreateBlogDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Sección")]
        public Guid SectionId { get; set; }

        public Data.Entities.Section? Section { get; set; }

        public List<SelectListItem>? Sections { get; set; }
    }
}
