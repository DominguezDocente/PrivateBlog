using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.Data.Seeders
{
    public class PermissionSeeder
    {
        private readonly DataContext _context;

        public PermissionSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.AddRange(Blogs());
            permissions.AddRange(Roles());
            permissions.AddRange(Users());
            permissions.AddRange(Setcions());

            foreach (Permission permission in permissions)
            {
                Permission? tmpPermission = _context.Permissions.Where(p => p.Name == permission.Name && p.Module == permission.Module)
                                                                .FirstOrDefault();
                if (tmpPermission is null)
                {
                    _context.Permissions.Add(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Blogs()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showBlogs", Description = "Ver Blogs", Module = "Blogs" },
                new Permission { Name = "createBlogs", Description = "Crear Blogs", Module = "Blogs" },
                new Permission { Name = "updateBlogs", Description = "Editar Blogs", Module = "Blogs" },
                new Permission { Name = "deleteBlogs", Description = "Eliminar Blogs", Module = "Blogs" },
            };

            return list;
        }

        private List<Permission> Roles()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showRoles", Description = "Ver Roles", Module = "Roles" },
                new Permission { Name = "createRoles", Description = "Crear Roles", Module = "Roles" },
                new Permission { Name = "updateRoles", Description = "Editar Roles", Module = "Roles" },
                new Permission { Name = "deleteRoles", Description = "Eliminar Roles", Module = "Roles" },
            };

            return list;
        }

        private List<Permission> Setcions()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showSections", Description = "Ver Secciones", Module = "Secciones" },
                new Permission { Name = "createSections", Description = "Crear Secciones", Module = "Secciones" },
                new Permission { Name = "updateSections", Description = "Editar Secciones", Module = "Secciones" },
                new Permission { Name = "deleteSections", Description = "Eliminar Secciones", Module = "Secciones" },
            };

            return list;
        }

        private List<Permission> Users()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showUsers", Description = "Ver Usuarios", Module = "Usuarios" },
                new Permission { Name = "createUsers", Description = "Crear Usuarios", Module = "Usuarios" },
                new Permission { Name = "updateUsers", Description = "Editar Usuarios", Module = "Usuarios" },
                new Permission { Name = "deleteUsers", Description = "Eliminar Usuarios", Module = "Usuarios" },
            };

            return list;
        }

    }
}
