using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class ToggleSectionStatusDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid SectionId { get; set; }

        public bool Hide { get; set; } = true;
    }
}
