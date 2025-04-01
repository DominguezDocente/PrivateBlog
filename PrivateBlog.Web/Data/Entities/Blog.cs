using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateBlog.Web.Data.Entities
{
    public class Blog : IId
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [MaxLength(64, ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "VARCHAR(MAX)")]
        [Display(Name = "Contenido")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Content { get; set; } = null!;

        [Display(Name = "¿Está publicado?")]
        public bool IsPublished { get; set; }

        public Section Section { get; set; }

        public int SectionId { get; set; }
    }
}
