using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PrivateBlog.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrivateBlog.Web.DTOs
{
    public class BlogDTO
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Titulo")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Title { get; set; }


        [Display(Name = "Contenido")]
        [Column(TypeName = "VARCHAR(MAX)")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Content { get; set; }

        public bool IsPublished { get; set; } = false;

        public Section? Section { get; set; }

        [Display(Name = "Sección")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sección")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int SectionId { get; set; }

        public IEnumerable<SelectListItem>? Sections { get; set; }

    }
}
