using Microsoft.AspNetCore.Mvc.Rendering;
using PrivateBlog.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateBlog.Web.DTOs
{
    public class BlogDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Blog")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; }

        [Display(Name = "Contenido")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Content { get; set; }

        public Section? Section { get; set; }

        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public Guid SectionId { get; set; }

        public List<SelectListItem>? Sections { get; set; }
    }
}
