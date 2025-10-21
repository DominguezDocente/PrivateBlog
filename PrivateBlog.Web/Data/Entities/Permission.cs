﻿using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class Permission
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(64)]
        [Required]
        public required string Name { get; set; }

        [MaxLength(128)]
        [Required]
        public required string Description { get; set; }

        [MaxLength(32)]
        [Required]
        public required string Module { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
