using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Section
{
    public class ToggleSectionStatusDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid Id { get; set; }

        public bool Hide { get; set; } = true;
    }
}
