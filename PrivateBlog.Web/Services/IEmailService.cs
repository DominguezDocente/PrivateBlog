using PrivateBlog.Web.Core;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Services
{
    public interface IEmailService
    {
        public Task<Response<object>> SendAsync(SendEmailDTO dto);
        public Task<Response<object>> SendResetPasswordEmailAsync(string email, string message, string resetTokenLink);
    }
}
