namespace PrivateBlog.Application.Contracts.Authentication
{
    /// <summary>
    /// Resultado de un intento de inicio de sesión (sin tipos de ASP.NET Identity).
    /// </summary>
    public sealed class AccountSignInResult
    {
        public required bool Succeeded { get; init; }

        public required bool IsLockedOut { get; init; }

        /// <summary>Credenciales incorrectas u otro fallo distinto de bloqueo.</summary>
        public bool HasInvalidCredentials => !Succeeded && !IsLockedOut;
    }
}
