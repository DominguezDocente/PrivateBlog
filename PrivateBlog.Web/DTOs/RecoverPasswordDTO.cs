using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class RecoverPasswordDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debes introducir un email válido.")]
        public string Email { get; set; } = null!;
    }
}
