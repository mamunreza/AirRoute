namespace AirRoute.Models
{
    public class Airport
    {
        public int AirportId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
