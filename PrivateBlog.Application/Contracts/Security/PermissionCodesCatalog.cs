using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Security
{
    public static class PermissionCodesCatalog
    {
        public const string SHOW_BLOGS = "showBlogs";
        public const string CREATE_BLOGS = "createBlogs";
        public const string EDIT_BLOGS = "editBlogs";
        public const string DELETE_BLOGS = "deleteBlogs";

        public const string SHOW_SECTIONS = "showSections";
        public const string CREATE_SECTIONS = "createSections";
        public const string EDIT_SECTIONS = "editSections";
        public const string DELETE_SECTIONS = "deleteSections";

        public const string SHOW_USERS = "showUsers";
        public const string CREATE_USERS = "createUsers";
        public const string EDIT_USERS = "editUsers";
        public const string DELETE_USERS = "deleteUsers";

        public const string SHOW_ROLES = "showRoles";
        public const string CREATE_ROLES = "createRoles";
        public const string EDIT_ROLES = "editRoles";
        public const string DELETE_ROLES = "deleteRoles";

        public readonly record struct PermissionSeed(string Code, string Description, string Module);

        public static IReadOnlyList<PermissionSeed> All { get; } = new List<PermissionSeed>
        {
            new PermissionSeed(SHOW_BLOGS, "Ver Blogs", "Blogs"),
            new PermissionSeed(CREATE_BLOGS, "Crear Blogs", "Blogs"),
            new PermissionSeed(EDIT_BLOGS, "Editar Blogs", "Blogs"),
            new PermissionSeed(DELETE_BLOGS, "Eliminar Blogs", "Blogs"),

            new PermissionSeed(SHOW_SECTIONS, "Ver Secciones", "Secciones"),
            new PermissionSeed(CREATE_SECTIONS, "Crear Secciones", "Secciones"),
            new PermissionSeed(EDIT_SECTIONS, "Editar Secciones", "Secciones"),
            new PermissionSeed(DELETE_SECTIONS, "Eliminar Secciones", "Secciones"),

            new PermissionSeed(SHOW_USERS, "Ver Usuarios", "Usuarios"),
            new PermissionSeed(CREATE_USERS, "Crear Usuarios", "Usuarios"),
            new PermissionSeed(EDIT_USERS, "Editar Usuarios", "Usuarios"),
            new PermissionSeed(DELETE_USERS, "Eliminar Usuarios", "Usuarios"),

            new PermissionSeed(SHOW_ROLES, "Ver Roles", "Roles"),
            new PermissionSeed(CREATE_ROLES, "Crear Roles", "Roles"),
            new PermissionSeed(EDIT_ROLES, "Editar Roles", "Roles"),
            new PermissionSeed(DELETE_ROLES, "Eliminar Roles", "Roles"),
        };

        public static IReadOnlyList<string> AllCodes { get; } = All.Select(p => p.Code).ToList();
    }
}
