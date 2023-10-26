using Microsoft.AspNetCore.Mvc;

namespace AirRoute.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Get health status");
            return Ok("Healthy");
        }
    }
}
