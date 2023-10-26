using AirRoute.Models;
using AirRoute.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirRoute.Controllers
{
    [Route("[controller]")]
    public class AirportsController : Controller
    {
        private readonly ILogger<AirportsController> _logger;
        private readonly IFileService _fileService;

        public AirportsController(
            ILogger<AirportsController> logger, 
            IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<Airport>> Index(string code)
        {
            _logger.LogInformation("Get airport information");
            var airportData = await _fileService.GetAirportsAsync(
                Path.Combine(AirNetworkConstants.RootDataDirectory, AirNetworkConstants.AirportDataFileName));

            return Ok(airportData.First(x => x.Iata == code));
        }
    }
}
