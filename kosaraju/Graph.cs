using System;
using System.Collections.Generic;
using System.IO;

namespace jrh.Algorithms.Kosaraju
{
    class Node
    {
        public int Number { get; private set; }
        public List<Node> ArrowTargets { get; private set; }
        public List<Node> ArrowSources { get; private set; }

        public bool Explored { get; private set; }

        public Node(int number)
        {
            ArrowSources = new List<Node>();
            ArrowTargets = new List<Node>();
            Number = number;
            SetUnexplored();
        }

        public void SetExplored()
        {
            if (Explored)
                throw new InvalidOperationException(string.Format("Setting node {0} as explored, but it already is", Number));

            Explored = true;
        }

        public void SetUnexplored()
        {
            Explored = false;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Number, Explored ? "Explored" : "Unexplored");
        }
    }

    class Graph
    {
        private List<Node> _nodes;
        private bool _isReversed;

        // Strongly connected components
        private Dictionary<Node, List<Node>> _SCCs;

        public Graph(List<Node> nodes)
        {
            _nodes = nodes;
            _SCCs = new Dictionary<Node, List<Node>>();

            SetForward();
        }

        // Adding an arrow adds two references, so we can use the reverse of this graph as efficiently as the forward graph
        public void AddArrow(int labelFrom, int labelTo)
        {
            Node source = GetNode(labelFrom);
            Node target = GetNode(labelTo);

            source.ArrowTargets.Add(target);
            target.ArrowSources.Add(source);
        }

        public Node GetNode(int label)
        {
            if (label < 1)
                throw new ArgumentOutOfRangeException("Label number needs to be >= 1");

            if (label > _nodes.Count)
                throw new ArgumentOutOfRangeException("Label number is too high");

            // For now, nodes are stored in order of their label (which is a number)
            int index = label - 1;

            return _nodes[index];
        }

        // Return the targets for a node, taking into account whether the graph is reversed
        public List<Node> GetTargets(Node node)
        {
            if (_isReversed)
                return node.ArrowSources;
            else
                return node.ArrowTargets;
        }

        public void AddNodeToSCC(Node leader, Node node)
        {
            if (!_SCCs.ContainsKey(leader))
                _SCCs.Add(leader, new List<Node>());

            // A strongly connected component's leader is obviously in the component, so no sense adding it
            if (leader == node)
                return;

            _SCCs[leader].Add(node);
        }

        public Dictionary<Node, List<Node>> GetSCCs()
        {
            return _SCCs;
        }

        public IEnumerable<Node> EnumerableNodes()
        {
            return _nodes;
        }

        public void SetUnexplored()
        {
            foreach (var node in _nodes)
                node.SetUnexplored();
        }

        public static Graph FromFile(string filename)
        {
            List<Tuple<int, int>> edges = new List<Tuple<int, int>>();

            int maxNodeNumber = 0;

            using (TextReader reader = File.OpenText(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var nums = line.Split(" ");
                    int source = int.Parse(nums[0]);
                    int target = int.Parse(nums[1]);

                    edges.Add(new Tuple<int, int>(source, target));

                    if (source > maxNodeNumber)
                        maxNodeNumber = source;

                    if (target > maxNodeNumber)
                        maxNodeNumber = target;
                }
            }

            List<Node> nodes = new List<Node>();

            for (int i = 1; i <= maxNodeNumber; i++)
                nodes.Add(new Node(i));

            Graph graph = new Graph(nodes);

            foreach (var edge in edges)
                graph.AddArrow(edge.Item1, edge.Item2);

            return graph;
        }

        public void SetReverse()
        {
            _isReversed = true;
        }

        public void SetForward()
        {
            _isReversed = false;
        }
    }
}