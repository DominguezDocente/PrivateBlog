using PrivateBlog.Web.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [FilterableAsString]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripción")]
        [FilterableAsString]
        public string? Description { get; set; }

        [Display(Name = "¿Está oculta?")]
        public bool IsHidden { get; set; }
    }
}
