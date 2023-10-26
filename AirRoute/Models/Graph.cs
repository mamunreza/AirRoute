namespace AirRoute.Models
{
    public class Graph
    {
        private List<Node> Nodes;

        public Graph()
        {
            Nodes = new List<Node>();
        }

        public void Add(Node n)
        {
            Nodes.Add(n);
        }

        public void Remove(Node n)
        {
            Nodes.Remove(n);
        }

        public List<Node> GetNodes()
        {
            return Nodes.ToList();
        }

        public int getCount()
        {
            return Nodes.Count;
        }
    }
}
