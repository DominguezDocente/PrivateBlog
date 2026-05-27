using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Api.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }

        /// <summary>
        /// false (default): devuelve JWT. true: inicia sesión con cookie de Identity.
        /// </summary>
        public bool UseCookie { get; set; }
    }
}
