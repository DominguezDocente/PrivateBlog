using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Users
{
    public sealed class CreateUserDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(64, MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(64, MinimumLength = 2)]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es válido.")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")]
        public Guid RoleId { get; set; }
    }
}
