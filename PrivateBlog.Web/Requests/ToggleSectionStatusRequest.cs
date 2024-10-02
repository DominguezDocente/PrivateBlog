namespace PrivateBlog.Web.Requests
{
    public class ToggleSectionStatusRequest
    {
        public int SectionId { get; set; }
        public bool Hide { get; set; }
    }
}
