using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Account
{
    public class EditProfileDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(64, MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(64, MinimumLength = 2)]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Correo")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [StringLength(32)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Rol")]
        public string RoleName { get; set; } = string.Empty;
    }
}
