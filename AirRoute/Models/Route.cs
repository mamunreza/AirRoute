namespace AirRoute.Models
{
    public class Route
    {
        public string? Airline { get; set; }
        public int? AirlineId { get; set; }
        public string Source { get; set; }
        public int? SourceId { get; set; }
        public string Destination { get; set; }
        public int? DestinationId { get; set; }
        public string? CodeShare { get; set; }
        public int? Stops { get; set; }
    }

    public class RouteWithDistance
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public double KmDistance { get; set; }
    }
}
