using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Entities.Account
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
                throw new BussinesRuleException("El nombre del rol es requerido.");
            }

            Name = name.Trim();
            Id = Guid.CreateVersion7();
        }
    }
}
