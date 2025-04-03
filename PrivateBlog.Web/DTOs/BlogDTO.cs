using Microsoft.AspNetCore.Mvc.Rendering;
using PrivateBlog.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateBlog.Web.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [MaxLength(64, ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "VARCHAR(MAX)")]
        [Display(Name = "Contenido")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Content { get; set; } = null!;

        [Display(Name = "¿Está publicado?")]
        public bool IsPublished { get; set; }

        public Section? Section { get; set; }

        [Display(Name = "Sección")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sección")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public int SectionId { get; set; }
        
        public IEnumerable<SelectListItem>? Sections { get; set; }
    }
}
