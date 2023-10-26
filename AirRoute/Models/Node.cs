namespace AirRoute.Models
{
    public class Node
    {
        private string Name;
        private string Iata;
        private Dictionary<Node, double> Neighbors;

        public Node(string NodeName, string iata)
        {
            this.Name = NodeName;
            Iata = iata;
            Neighbors = new Dictionary<Node, double>();
        }

        public void AddNeighbour(Node n, double distance)
        {
            Neighbors.Add(n, distance);
        }

        public string GetName()
        {
            return Name;
        }

        public string GetIata()
        {
            return Iata;
        }

        public Dictionary<Node, double> GetNeighbors()
        {
            return Neighbors;
        }
    }

    public class BackTrackNode
    {
        public Node CurrentNode { get; set; }
        public Node PreviousNode { get; set; }
    }
}