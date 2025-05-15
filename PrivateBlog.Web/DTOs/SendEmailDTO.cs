using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class SendEmailDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "Debe introducir un email válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Content { get; set; } = null!;
    }
}
