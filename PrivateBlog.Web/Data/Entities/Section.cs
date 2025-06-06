﻿using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class Section : IId
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sección")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "¿Está oculta?")]
        public bool IsHidden { get; set; }

        public ICollection<RoleSection>? RoleSections { get; set; }
    }
}
