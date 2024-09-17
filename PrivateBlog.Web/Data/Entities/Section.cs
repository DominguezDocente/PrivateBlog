using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "¿Está Oculta?")]
        public bool IsHidden { get; set; }
    }
}
