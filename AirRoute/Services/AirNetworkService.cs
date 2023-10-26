using AirRoute.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AirRoute.Services
{
    public interface IAirNetworkService
    {
        Task<Graph> DefineGraphAsync(string key);
        Task<List<Airport>> GetAirportsInfoAsync();
    }

    public class AirNetworkService : IAirNetworkService
    {
        private readonly ILogger<AirNetworkService> _logger;
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;

        public AirNetworkService(
            ILogger<AirNetworkService> logger,
            IFileService fileService,
            IMemoryCache cache)
        {
            _logger = logger;
            _fileService = fileService;
            _cache = cache;
        }

        public async Task<Graph> DefineGraphAsync(string key)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(30));

            if (_cache.TryGetValue(key, out Graph airNetwork))
            {
                return airNetwork;
            }

            airNetwork = new Graph();

            var airports = await GetAirportsInfoAsync();


            _logger.LogInformation("Get route distance information from storage");
            var routeWithDistances =
                await _fileService.GetRouteWithDistancesAsync(
                    Path.Combine(
                        AirNetworkConstants.RootDataDirectory,
                        AirNetworkConstants.RouteDistanceDataFileName));
            routeWithDistances.RemoveAll(x => x.KmDistance < 0);

            _logger.LogInformation("Defining the air network graph");
            foreach (var airport in airports)
            {
                try
                {
                    var node = new Node(airport.Name, airport.Iata);
                    foreach (var route in routeWithDistances)
                    {
                        if (route.Source == airport.Iata)
                        {
                            var neighbor = airports.First(x => x.Iata == route.Destination);
                            node.AddNeighbour(
                                new Node(neighbor.Name, neighbor.Iata),
                                route.KmDistance);
                        }
                    }
                    airNetwork.Add(node);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error defining the air network graph", e.Message);
                }
            }

            _cache.Set(key, airNetwork, cacheOptions);

            return airNetwork;
        }

        public async Task<List<Airport>> GetAirportsInfoAsync()
        {
            _logger.LogInformation("Get airport information from storage");
            var airports = await _fileService.GetAirportsAsync(
                Path.Combine(AirNetworkConstants.RootDataDirectory, AirNetworkConstants.AirportDataFileName));
            airports.RemoveAll(x => x.Iata == string.Empty);
            airports.RemoveAll(x => x.Iata == "\\N");
            return airports;
        }
    }
}
