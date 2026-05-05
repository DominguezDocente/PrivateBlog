using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Entities.Account
{
    public sealed class Permission
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public string Module { get; private set; }
        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

        private Permission() { }

        public Permission(string code, string description, string module)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BussinesRuleException("El código es requerido.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new BussinesRuleException("La descripción es requerida.");
            }

            if (string.IsNullOrWhiteSpace(module))
            {
                throw new BussinesRuleException("El módulo es requerido.");
            }

            Id = Guid.CreateVersion7();
            Code = code;
            Description = description;
            Module = module;
        }
    }
}
