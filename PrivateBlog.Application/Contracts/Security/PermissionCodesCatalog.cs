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

        public readonly record struct PermissionSeed(string Code, string Description, string Module);

        public static IReadOnlyList<PermissionSeed> All { get; } = new List<PermissionSeed>
        {
            new PermissionSeed(SHOW_BLOGS, "Show Blogs", "Blogs"),
            new PermissionSeed(CREATE_BLOGS, "Create Blogs", "Blogs"),
            new PermissionSeed(EDIT_BLOGS, "Edit Blogs", "Blogs"),
            new PermissionSeed(DELETE_BLOGS, "Delete Blogs", "Blogs"),

            new PermissionSeed(SHOW_SECTIONS, "Show Sections", "Secciones"),
            new PermissionSeed(CREATE_SECTIONS, "Create Sections", "Secciones"),
            new PermissionSeed(EDIT_SECTIONS, "Edit Sections", "Secciones"),
            new PermissionSeed(DELETE_SECTIONS, "Delete Sections", "Secciones")
        };

        public static IReadOnlyList<string> AllCodes { get; } = All.Select(p => p.Code).ToList();
    }
}
