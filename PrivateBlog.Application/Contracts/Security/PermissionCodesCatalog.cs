using PrivateBlog.Domain.Entities.Account;

namespace PrivateBlog.Application.Contracts.Security
{
    public static class PermissionCodesCatalog
    {
        public const string ShowBlogs = "showBlogs";
        public const string CreateBlogs = "createBlogs";
        public const string UpdateBlogs = "updateBlogs";
        public const string DeleteBlogs = "deleteBlogs";

        public const string ShowSections = "showSections";
        public const string CreateSections = "createSections";
        public const string UpdateSections = "updateSections";
        public const string DeleteSections = "deleteSections";

        public const string ShowRoles = "showRoles";
        public const string CreateRoles = "createRoles";
        public const string UpdateRoles = "updateRoles";
        public const string DeleteRoles = "deleteRoles";

        public const string ShowUsers = "showUsers";
        public const string CreateUsers = "createUsers";
        public const string UpdateUsers = "updateUsers";
        public const string DeleteUsers = "deleteUsers";

        public readonly record struct PermissionSeed(string Code, string Description, PermissionModule Module);

        public static IReadOnlyList<PermissionSeed> All { get; } =
        [
            new(ShowBlogs, "Ver blogs", PermissionModule.Blogs),
            new(CreateBlogs, "Crear blogs", PermissionModule.Blogs),
            new(UpdateBlogs, "Editar blogs", PermissionModule.Blogs),
            new(DeleteBlogs, "Eliminar blogs", PermissionModule.Blogs),
            new(ShowSections, "Ver secciones", PermissionModule.Secciones),
            new(CreateSections, "Crear secciones", PermissionModule.Secciones),
            new(UpdateSections, "Editar secciones", PermissionModule.Secciones),
            new(DeleteSections, "Eliminar secciones", PermissionModule.Secciones),
            new(ShowRoles, "Ver roles", PermissionModule.Roles),
            new(CreateRoles, "Crear roles", PermissionModule.Roles),
            new(UpdateRoles, "Editar roles", PermissionModule.Roles),
            new(DeleteRoles, "Eliminar roles", PermissionModule.Roles),
            new(ShowUsers, "Ver usuarios", PermissionModule.Usuarios),
            new(CreateUsers, "Crear usuarios", PermissionModule.Usuarios),
            new(UpdateUsers, "Editar usuarios", PermissionModule.Usuarios),
            new(DeleteUsers, "Eliminar usuarios", PermissionModule.Usuarios),
        ];

        public static IReadOnlyList<string> AllCodes { get; } = All.Select(static s => s.Code).ToArray();

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string ContentEditor = "Editor de contenido";
            public const string Basic = "Básico";
        }

        public static class RoleGrants
        {
            public static IReadOnlyList<string> Admin { get; } = AllCodes;

            public static IReadOnlyList<string> ContentEditor { get; } =
            [
                ShowBlogs, CreateBlogs, UpdateBlogs, DeleteBlogs,
                ShowSections, CreateSections, UpdateSections, DeleteSections,
            ];

            public static IReadOnlyList<string> Basic { get; } = [ShowBlogs, ShowSections];
        }
    }
}
