using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        public IActionResult Index(DateTime? date)
        {
            DateTime selectedDate = date ?? DateTime.Today;
            string logFileName = $"log{selectedDate:yyyyMMdd}.log";
            string logFilePath = Path.Combine("logs", logFileName);

            LogViewerDTO model = new LogViewerDTO
            {
                SelectedDate = selectedDate
            };

            if (System.IO.File.Exists(logFilePath))
            {
                List<LogDTO> entries = _logService.GetLogsByPath(logFilePath);
                model.Logs = entries.OrderByDescending(e => e.Timestamp).ToList();
            }

            return View(model);

            //List<DTOs.LogDTO> logs = _logService.GetLogs(DateTime.Today);
            //return View(logs);
        }
    }
}
