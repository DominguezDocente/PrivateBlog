using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.DTOs
{
    public class SectionForDTO : Section
    {
        public bool Selected { get; set; } = false;
    }
}