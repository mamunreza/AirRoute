using System.Text.RegularExpressions;
using AirRoute.Models;
using Route = AirRoute.Models.Route;

namespace AirRoute.Services
{
    public interface IFileService
    {
        Task<List<Airport>> GetAirportsAsync(string path);
        Task<List<Route>> GetRoutesAsync(string path);
        Task<List<RouteWithDistance>> GetRouteWithDistancesAsync(string path);
    }

    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public async Task<List<Airport>> GetAirportsAsync(string path)
        {
            var list = new List<Airport>();
            
            _logger.LogInformation("Get airport information from storage");
            var lines = await File.ReadAllLinesAsync(path);

            for (int i = 0; i < lines.Length; i++)
            {
                var fields = Regex.Split(lines[i], "[,]{1}(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                try
                {
                    list.Add(new Airport
                    {
                        AirportId = Convert.ToInt32(fields[0]),
                        Name = fields[1].Trim('\"'),
                        City = fields[2].Trim('\"'),
                        Country = fields[3].Trim('\"'),
                        Iata = fields[4].Trim('\"'),
                        Icao = fields[5].Trim('\"'),
                        Latitude = Convert.ToDouble(fields[6]),
                        Longitude = Convert.ToDouble(fields[7])
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("Error getting airport information from storage", e.Message);
                }
            }

            return list;
        }

        public async Task<List<Route>> GetRoutesAsync(string path)
        {
            var list = new List<Route>();

            _logger.LogInformation("Get route information from storage");
            var lines = await File.ReadAllLinesAsync(path);

            for (int i = 0; i < lines.Length; i++)
            {
                var fields = Regex.Split(lines[i], "[,]{1}(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                try
                {
                    list.Add(new Route
                    {
                        Source = fields[2],
                        Destination = fields[4]
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("Error getting route information from storage", e.Message);
                }
            }

            return list;
        }

        public async Task<List<RouteWithDistance>> GetRouteWithDistancesAsync(string path)
        {
            var list = new List<RouteWithDistance>();

            _logger.LogInformation("Get route distance information from storage");
            var lines = await File.ReadAllLinesAsync(path);

            for (int i = 0; i < lines.Length; i++)
            {
                var fields = Regex.Split(lines[i], "[,]{1}(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                try
                {
                    list.Add(new RouteWithDistance
                    {
                        Source = fields[0],
                        Destination = fields[1],
                        KmDistance = Convert.ToDouble(fields[2])
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("Error getting route distance information from storage", e.Message);
                }
            }

            return list;
        }
    }
}
