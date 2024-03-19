using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class Section
    {
        public int Id { get; set; }

        [Display(Name="Sección")]
        [Required( ErrorMessage = "El campo '{0}' es requerido.")]
        [MaxLength(64, ErrorMessage = "El campo '{0}' debe terner máximo {1} caractéres")]
        public string Name { get; set; }

        public bool IsHidden { get; set; } = false;
    }
}
