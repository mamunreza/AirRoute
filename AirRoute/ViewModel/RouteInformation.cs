namespace AirRoute.ViewModel
{
    public class RouteInformation
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public double Distance { get; set; }
        public List<string> Hops { get; set; }
    }
}
