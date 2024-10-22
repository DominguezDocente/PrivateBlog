using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data.Entities;
using static System.Collections.Specialized.BitVector32;

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
            List<Permission> permissions = [.. Blogs(), .. Sections()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Name == permission.Name 
                                                                        && p.Module == permission.Module);

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
                new Permission { Name = "showBlogs", Description = "Ver Blogs", Module = "Blogs" },
                new Permission { Name = "createBlogs", Description = "Crear Blogs", Module = "Blogs" },
                new Permission { Name = "editBlogs", Description = "Editar Blogs", Module = "Blogs" },
                new Permission { Name = "deleteBlogs", Description = "Eliminar Blogs", Module = "Blogs" },
            };
        }

        private List<Permission> Sections()
        {
            return new List<Permission>
            {
                new Permission { Name = "showSections", Description = "Ver Sections", Module = "Sections" },
                new Permission { Name = "createSections", Description = "Crear Sections", Module = "Sections" },
                new Permission { Name = "editSections", Description = "Editar Sections", Module = "Sections" },
                new Permission { Name = "deleteSections", Description = "Eliminar Sections", Module = "Sections" },
            };
        }
    }
}
