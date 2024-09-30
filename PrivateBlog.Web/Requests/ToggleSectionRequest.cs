namespace PrivateBlog.Web.Requests
{
    public class ToggleSectionRequest
    {
        public int SectionId { get; set; }
        public bool Hide { get; set; }
    }
    
}
