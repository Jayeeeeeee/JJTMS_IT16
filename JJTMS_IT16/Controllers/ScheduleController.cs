using Microsoft.AspNetCore.Mvc;

namespace JJTMS_IT16.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly HttpClient _httpClient;

        public SchedulerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> StartPythonScheduler()
        {
            var response = await _httpClient.GetAsync("http://localhost:5000/start_scheduler");
            if (response.IsSuccessStatusCode)
            {
                return Content("Python scheduler started.");
            }
            return Content("Failed to start Python scheduler.");
        }

        public async Task<IActionResult> StopPythonScheduler()
        {
            var response = await _httpClient.GetAsync("http://localhost:5000/stop_scheduler");
            if (response.IsSuccessStatusCode)
            {
                return Content("Python scheduler stopped.");
            }
            return Content("Failed to stop Python scheduler.");
        }
    }
}
