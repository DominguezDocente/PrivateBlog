using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Entities.Account
{
    public sealed class Permission
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public PermissionModule Module { get; private set; }

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

        private Permission()
        {
        }

        public Permission(string code, string description, PermissionModule module)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BussinesRuleException("El código del permiso es requerido.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new BussinesRuleException("La descripción del permiso es requerida.");
            }

            Code = code.Trim();
            Description = description.Trim();
            Module = module;
            Id = Guid.CreateVersion7();
        }
    }
}
