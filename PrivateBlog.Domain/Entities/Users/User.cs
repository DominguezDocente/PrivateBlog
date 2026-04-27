using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Domain.Entities.Users
{
    /// <summary>
    /// Usuario del sistema (dominio puro). La autenticación se implementa en infraestructura con un tipo propio de ASP.NET Identity, mapeado a este modelo cuando el caso de uso trabaja con dominio.
    /// </summary>
    public sealed class User
    {
        public string Id { get; private set; } = null!;

        public string Email { get; private set; } = null!;

        public string UserName { get; private set; } = null!;

        public bool EmailConfirmed { get; private set; }

        public string? PhoneNumber { get; private set; }

        public bool LockoutEnabled { get; private set; }

        public DateTimeOffset? LockoutEndUtc { get; private set; }

        private User()
        {
        }

        public static User Reconstitute(
            string id,
            string email,
            string userName,
            bool emailConfirmed,
            string? phoneNumber,
            bool lockoutEnabled,
            DateTimeOffset? lockoutEndUtc)
        {
            ValidateId(id);
            ValidateEmail(email);
            ValidateUserName(userName);

            return new User
            {
                Id = id.Trim(),
                Email = email.Trim(),
                UserName = userName.Trim(),
                EmailConfirmed = emailConfirmed,
                PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim(),
                LockoutEnabled = lockoutEnabled,
                LockoutEndUtc = lockoutEndUtc,
            };
        }

        public void UpdateContact(string email, string? phoneNumber)
        {
            ValidateEmail(email);
            Email = email.Trim();
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
        }

        public void UpdateUserName(string userName)
        {
            ValidateUserName(userName);
            UserName = userName.Trim();
        }

        public void SetEmailConfirmed(bool confirmed)
        {
            EmailConfirmed = confirmed;
        }

        private static void ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BusinessRuleException("El identificador del usuario es requerido.");
            }
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessRuleException("El correo es requerido.");
            }

            string trimmed = email.Trim();

            if (trimmed.Length > 256)
            {
                throw new BusinessRuleException("El correo no puede superar los 256 caracteres.");
            }
        }

        private static void ValidateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new BusinessRuleException("El nombre de usuario es requerido.");
            }

            string trimmed = userName.Trim();

            if (trimmed.Length > 256)
            {
                throw new BusinessRuleException("El nombre de usuario no puede superar los 256 caracteres.");
            }
        }
    }
}
