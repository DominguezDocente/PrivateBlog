using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Domain.Entities.Users
{
    public sealed class Permission
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } = null!;
        public string Description { get; private set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

        private Permission()
        {
        }

        public Permission(string code, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BusinessRuleException("El código del permiso es requerido.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new BusinessRuleException("La descripción del permiso es requerida.");
            }

            Code = code.Trim();
            Description = description.Trim();
            Id = Guid.CreateVersion7();
        }
    }
}
