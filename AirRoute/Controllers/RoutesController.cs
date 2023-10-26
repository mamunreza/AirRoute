using AirRoute.Models;
using AirRoute.Services;
using AirRoute.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AirRoute.Controllers
{
    [Route("[controller]")]
    public class RoutesController : Controller
    {
        private readonly ILogger<RoutesController> _logger;
        private readonly IAirNetworkService _airNetworkService;

        public RoutesController(
            ILogger<RoutesController> logger,
            IFileService fileService,
            IAirNetworkService airNetworkService)
        {
            _logger = logger;
            _airNetworkService = airNetworkService;
        }

        [HttpGet]
        [Route("{source}/{destination}")]
        public async Task<ActionResult<RouteInformation>> Index(string source, string destination)
        {
            var airportData = await _airNetworkService.GetAirportsInfoAsync();
            var airNetworkGraph = await _airNetworkService.DefineGraphAsync("airnetwork");

            var distanceCalculator = new DistanceCalculator(airNetworkGraph);

            var departureAirport = airportData.FirstOrDefault(x => x.Iata == source)
                                   ?? airportData.FirstOrDefault(y => y.Icao == source);
            if (departureAirport == null)
            {
                return BadRequest("Please provide correct source IATA/ICAO");
            }

            var destinationAirport = airportData.FirstOrDefault(x => x.Iata == destination)
                                     ?? airportData.FirstOrDefault(y => y.Icao == destination);
            if (destinationAirport == null)
            {
                return BadRequest("Please provide correct destination IATA/ICAO");
            }

            var departure = new Node(departureAirport.Name, departureAirport.Iata);
            var dest = new Node(destinationAirport.Name, destinationAirport.Iata);

            _logger.LogInformation("Generating route information");
            var routeInfo = distanceCalculator.Execute(departure, dest);

            return Ok(routeInfo);
        }
    }
}
