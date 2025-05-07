namespace PrivateBlog.Web.Services
{
    public interface IStorageService
    {
        public Task DeleteFile(string path, string container);
        public Task<string> SaveFileAsync(byte[] content, string extension, string container, string contentType);
        public Task<string> UpdateFile(byte[] content, string extension, string container, string path, string contentType);
    }

}
