using AirRoute.Models;
using AirRoute.ViewModel;

namespace AirRoute.Services
{
    public class DistanceCalculator
    {
        readonly Dictionary<string, double> _distances;
        readonly Graph _graph;
        readonly List<Node> _allNodes;
        readonly List<BackTrackNode> _possibleHops;

        public DistanceCalculator(Graph graph)
        {
            _graph = graph;
            _allNodes = graph.GetNodes();
            _distances = SetDistances();
            _possibleHops = new List<BackTrackNode>();
        }

        public RouteInformation Execute(Node source, Node destination)
        {
            _distances[source.GetIata()] = 0;

            while (_allNodes.ToList().Count != 0)
            {
                Node leastDistantNode = GetLeastDistanceNode();
                ExamineConnections(leastDistantNode);
                _allNodes.Remove(leastDistantNode);
            }

            return GetRouteInfo(source, destination);
        }

        private Dictionary<string, double> SetDistances()
        {
            var distances = new Dictionary<string, double>();

            foreach (Node n in _graph.GetNodes())
            {
                distances.Add(n.GetIata(), double.MaxValue);
            }

            return distances;
        }

        private void ExamineConnections(Node n)
        {

            foreach (var neighbor in n.GetNeighbors())
            {
                var distance = _distances[neighbor.Key.GetIata()];
                var currentDistance = _distances[n.GetIata()];
                if (currentDistance + neighbor.Value < distance)
                {
                    _distances[neighbor.Key.GetIata()] = neighbor.Value + currentDistance;
                    _possibleHops.Add(new BackTrackNode
                    {
                        CurrentNode = neighbor.Key,
                        PreviousNode = n
                    });
                }
            }
        }

        private Node GetLeastDistanceNode()
        {
            var leastDistance = _allNodes.First();

            foreach (var n in _allNodes)
            {
                if (_distances[n.GetIata()] < _distances[leastDistance.GetIata()])
                    leastDistance = n;
            }

            _possibleHops.Add(new BackTrackNode
            {
                CurrentNode = leastDistance,
                PreviousNode = null
            });
            return leastDistance;
        }

        private RouteInformation GetRouteInfo(Node source, Node destination)
        {
            return new RouteInformation
            {
                Source = source.GetIata(),
                Destination = destination.GetIata(),
                Distance = _distances[destination.GetIata()],
                Hops = GetHops(destination)
            };
        }

        private List<string> GetHops(Node currentNode)
        {
            var hops = new List<string>();
            var finalRoute = new List<BackTrackNode>();
            var node = _possibleHops.Find(x => x.CurrentNode.GetIata() == currentNode.GetIata());
            if (node == null)
            {
                return new List<string>();
            }

            finalRoute.Add(node);
            do
            {
                var anotherNode = _possibleHops.Find(x =>
                    x.CurrentNode.GetIata() == node.PreviousNode?.GetIata());
                if (anotherNode == null)
                    break;
                finalRoute.Add(anotherNode);
                node = anotherNode;
            } while (true);

            finalRoute.Reverse();
            finalRoute.ForEach(x => hops.Add(x.CurrentNode.GetIata()));
            return hops;
        }
    }
}