using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Entities.Account
{
    public class Role
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

        private Role() { }

        public Role(string name)
        {
            CheckNameValidation(name);

            Id = Guid.CreateVersion7();
            Name = name;
        }

        public void UpdateName(string name)
        {
            CheckNameValidation(name);

            Name = name;
        }

        private void CheckNameValidation(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre es requerido.");
            }

            if (name.Length > 64)
            {
                throw new BussinesRuleException("El nombre no puede exceder los 64 caracteres.");
            }

            if (name.Length < 3)
            {
                throw new BussinesRuleException("El nombre debe tener al menos 3 caracteres.");
            }
        }
    }
}
