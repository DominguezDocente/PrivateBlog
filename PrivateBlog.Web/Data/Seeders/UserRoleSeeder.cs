using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Data.Seeders
{
    public class UserRoleSeeder
    {
        private readonly IUsersService _usersService;
        private readonly DataContext _context;

        public UserRoleSeeder(IUsersService usersService, DataContext context)
        {
            _usersService = usersService;
            _context = context;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsers();
        }

        private async Task AdministradorRoleAsync()
        {
            PrivateBlogRole? tmp = await _context.PrivateBlogRoles.Where(ir => ir.Name == "Administrador").FirstOrDefaultAsync();

            if (tmp == null)
            {
                PrivateBlogRole role = new PrivateBlogRole { Name = "Administrador" };
                _context.PrivateBlogRoles.Add(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckUsers()
        {
            // Administrador
            User? user = await _usersService.GetUserAsync("manueld@yopmail.com");

            PrivateBlogRole adminRole = _context.PrivateBlogRoles.Where(r => r.Name == "Administrador")
                                                                 .First();

            if (user is null)
            {
                user = new User
                {
                    Email = "manueld@yopmail.com",
                    FirstName = "Manuel Alejandro",
                    LastName = "Domínguez Guerrero",
                    PhoneNumber = "3000000000",
                    UserName = "manueld@yopmail.com",
                    Document = "1111",
                    PrivateBlogRole = adminRole,
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Creador de contenido
            user = await _usersService.GetUserAsync("anad@yopmail.com");


            PrivateBlogRole creadorDeContenidoRole = await _context.PrivateBlogRoles.Where(pbr => pbr.Name == "Creador de contenido")
                                                                                    .FirstAsync();

            if (user == null)
            {
                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "30000000",
                    UserName = "anad@yopmail.com",
                    Document = "2222",
                    PrivateBlogRole = creadorDeContenidoRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Gestor de usuarios
            user = await _usersService.GetUserAsync("jhond@yopmail.com");

            PrivateBlogRole gestorDeUsuarios = await _context.PrivateBlogRoles.Where(pbr => pbr.Name == "Gestor de usuarios")
                                                                              .FirstAsync();

            if (user == null)
            {
                user = new User
                {
                    Email = "jhond@yopmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe",
                    PhoneNumber = "30000000",
                    UserName = "jhond@yopmail.com",
                    Document = "3333",
                    PrivateBlogRole = gestorDeUsuarios
                };

                var result = await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CreadorDeContenidoRoleAsync()
        {

            PrivateBlogRole? tmp = await _context.PrivateBlogRoles.Where(pbr => pbr.Name == "Creador de contenido")
                                                                  .FirstOrDefaultAsync();

            if (tmp == null)
            {
                PrivateBlogRole role = new PrivateBlogRole { Name = "Creador de contenido" };

                _context.PrivateBlogRoles.Add(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Secciones").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    _context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task GestorDeUsuariosRoleAsync()
        {

            PrivateBlogRole? tmp = await _context.PrivateBlogRoles.Where(pbr => pbr.Name == "Gestor de usuarios")
                                                                  .FirstOrDefaultAsync();

            if (tmp == null)
            {
                PrivateBlogRole role = new PrivateBlogRole { Name = "Gestor de usuarios" };

                _context.PrivateBlogRoles.Add(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Usuarios").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    _context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }

            await _context.SaveChangesAsync();
        }


        private async Task CheckRolesAsync()
        {
            await AdministradorRoleAsync();
            await CreadorDeContenidoRoleAsync();
            await GestorDeUsuariosRoleAsync();
        }
    }
}
