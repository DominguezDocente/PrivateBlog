using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Section
{
    public class ToggleSectionStatusDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid SectionId { get; set; }

        public bool Hide { get; set; } = true;
    }
}
