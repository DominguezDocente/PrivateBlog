namespace PrivateBlog.Web.Data.Entities
{
    public class RoleSection
    {
        public required Guid PrivateBlogRoleId { get; set; }
        public required Guid SectionId { get; set; }
        public PrivateBlogRole PrivateBlogRole { get; set; }
        public Section Section { get; set; }
    }
}
