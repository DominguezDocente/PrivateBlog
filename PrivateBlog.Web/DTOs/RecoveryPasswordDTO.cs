using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class RecoveryPasswordDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "Debe introducir un email válido.")]
        public string Email { get; set; } = null!;
    }
}
