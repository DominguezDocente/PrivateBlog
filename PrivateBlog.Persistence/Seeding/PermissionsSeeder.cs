using Microsoft.EntityFrameworkCore;
using PrivateBlog.Domain.Entities.Users;

namespace PrivateBlog.Persistence.Seeding
{
    internal sealed class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            (string Code, string Description)[] permissions =
            [
                ..Blogs(),
                ..Sections(),
                ..Roles(),
                ..Users(),
            ];

            foreach ((string code, string description) in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Code == code);
                if (!exists)
                {
                    await _context.Permissions.AddAsync(new Permission(code, description));
                }
            }

            await _context.SaveChangesAsync();
        }

        private static (string Code, string Description)[] Blogs()
        {
            return
            [
                ("showBlogs", "Ver blogs"),
                ("createBlogs", "Crear blogs"),
                ("updateBlogs", "Editar blogs"),
                ("deleteBlogs", "Eliminar blogs"),
            ];
        }

        private static (string Code, string Description)[] Sections()
        {
            return
            [
                ("showSections", "Ver secciones"),
                ("createSections", "Crear secciones"),
                ("updateSections", "Editar secciones"),
                ("deleteSections", "Eliminar secciones"),
            ];
        }

        private static (string Code, string Description)[] Roles()
        {
            return
            [
                ("showRoles", "Ver roles"),
                ("createRoles", "Crear roles"),
                ("updateRoles", "Editar roles"),
                ("deleteRoles", "Eliminar roles"),
            ];
        }

        private static (string Code, string Description)[] Users()
        {
            return
            [
                ("showUsers", "Ver usuarios"),
                ("createUsers", "Crear usuarios"),
                ("updateUsers", "Editar usuarios"),
                ("deleteUsers", "Eliminar usuarios"),
            ];
        }
    }
}
