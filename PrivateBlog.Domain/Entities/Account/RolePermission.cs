using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Entities.Account
{
    public class RolePermission
    {
        public Guid RoleId { get; private set; }
        public Guid PermissionId { get; private set; }
        public Role Role { get; private set; }
        public Permission Permission { get; private set; }

        private RolePermission()
        {
            
        }

        public RolePermission(Guid roleId, Guid permissionId)
        {
            if (roleId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id del rol es requerido.");
            }
            if (permissionId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id del permiso es requerido.");
            }

            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
