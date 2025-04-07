namespace PrivateBlog.Web.DTOs
{
    public class LogDTO
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string? Exception { get; set; }
        public string Method { get; set; }

    }
}
