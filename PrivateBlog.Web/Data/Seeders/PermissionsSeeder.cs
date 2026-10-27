using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.Data.Seeders
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = [ ..Blogs(), ..Sections(), ..Users(), ..Roles() ];

            foreach(Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Code == permission.Code);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Blogs()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Code = "blogs.show", Description = "Ver Blogs", Module = "Blogs"},
                new Permission { Id = Guid.NewGuid(), Code = "blogs.create", Description = "Crear Blogs", Module = "Blogs"},
                new Permission { Id = Guid.NewGuid(), Code = "blogs.update", Description = "Editar Blogs", Module = "Blogs"},
                new Permission { Id = Guid.NewGuid(), Code = "blogs.delete", Description = "Eliminar Blogs", Module = "Blogs"},
            };
        }

        private List<Permission> Sections()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Code = "sections.show", Description = "Ver Secciones", Module = "Secciones"},
                new Permission { Id = Guid.NewGuid(), Code = "sections.create", Description = "Crear Secciones", Module = "Secciones"},
                new Permission { Id = Guid.NewGuid(), Code = "sections.update", Description = "Editar Secciones", Module = "Secciones"},
                new Permission { Id = Guid.NewGuid(), Code = "sections.delete", Description = "Eliminar Secciones", Module = "Secciones"},
            };
        }

        private List<Permission> Users()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Code = "users.show", Description = "Ver Usuarios", Module = "Usuarios"},
                new Permission { Id = Guid.NewGuid(), Code = "users.create", Description = "Crear Usuarios", Module = "Usuarios"},
                new Permission { Id = Guid.NewGuid(), Code = "users.update", Description = "Editar Usuarios", Module = "Usuarios"},
                new Permission { Id = Guid.NewGuid(), Code = "users.delete", Description = "Eliminar Usuarios", Module = "Usuarios"},
            };
        }

        private List<Permission> Roles()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Code = "roles.show", Description = "Ver Roles", Module = "Roles"},
                new Permission { Id = Guid.NewGuid(), Code = "roles.create", Description = "Crear Roles", Module = "Roles"},
                new Permission { Id = Guid.NewGuid(), Code = "roles.update", Description = "Editar Roles", Module = "Roles"},
                new Permission { Id = Guid.NewGuid(), Code = "roles.delete", Description = "Eliminar Roles", Module = "Roles"},
            };
        }
    }
}
