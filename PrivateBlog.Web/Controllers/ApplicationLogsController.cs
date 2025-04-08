using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class ApplicationLogsController : Controller
    {
        private readonly IReadLogsService _readLogsService;

        public ApplicationLogsController(IReadLogsService readLogsService)
        {
            _readLogsService = readLogsService;
        }

        public IActionResult Index(DateTime? date)
        {
            LogViewerDTO dto = new LogViewerDTO
            {
                Logs = _readLogsService.GetLogs(date),
                SelectedDate = date
            };

            return View(dto);
        }
    }
}
