using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Domain.Entities.Users
{
    public sealed class Role
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

        private Role()
        {
        }

        public Role(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleException("El nombre del rol es requerido.");
            }

            Name = name.Trim();
            Id = Guid.CreateVersion7();
        }
    }
}
