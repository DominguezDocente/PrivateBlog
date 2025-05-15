using PrivateBlog.Web.Core;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Requests.Mailtrap;

namespace PrivateBlog.Web.Services
{
    public class MailtrapService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IApiService _apiService;

        public MailtrapService(IConfiguration configuration, IWebHostEnvironment env, IApiService apiService)
        {
            _configuration = configuration;
            _env = env;
            _apiService = apiService;
        }

        public async Task<Response<object>> SendAsync(SendEmailDTO dto)
        {
            try
            {
                string? apiKey = _configuration["MailtrapApiKey"];

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new ApplicationException("No está registrado el ApiLey de Mailtrap en el archivo de configuración");
                }

                SendEmailRequest request = new SendEmailRequest
                {
                    from = new From
                    {
                        name = "Equipo PrivateBlog",
                        email = "no-reply@privateblog.com"
                    },

                    to = 
                    [                      
                        new To { email = dto.Email },
                    ],

                    subject = dto.Subject,

                    text = dto.Content,
                };

                string url = "https://sandbox.api.mailtrap.io/api/send/2846072";
                List<HeaderItem> headers = [ new HeaderItem { Name = "Authorization", Value = $"Bearer {apiKey}" } ];

                return await _apiService.PostAsync<object>(url, request, headers);

            }
            catch(Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<object>> SendResetPasswordEmailAsync(string email, string message, string resetTokenLink)
        {
            try
            {
                string filePath = Path.Combine(_env.ContentRootPath, "Emails", "ResetPasswordTemplate.html");
                string html = File.ReadAllText(filePath);

                html = html.Replace("{{UserName}}", email)
                           .Replace("{{Message}}", message)
                           .Replace("{{ResetTokenLink}}", resetTokenLink);

                return await SendAsync(new SendEmailDTO 
                {
                    Content = html,
                    Email = email,
                    Subject = "Restablecimiento de contraseña"
                });
            }
            catch(Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }
    }
}
