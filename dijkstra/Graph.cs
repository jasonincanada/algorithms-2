using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace jrh.Algorithms.Dijkstra
{
    class WeightedEdge
    {
        public Vertex Source { get; private set; }
        public Vertex Target { get; private set; }
        public long Weight { get; private set; }

        public WeightedEdge(Vertex source, Vertex target, long weight)
        {
            Source = source;
            Target = target;
            Weight = weight;
        }

        public override string ToString()
        {
            return string.Format("--{0}--> {1}", Weight, Target.Number);
        }
    }

    class Vertex
    {
        public int Number { get; private set; }
        public bool Explored { get; private set; }
        public long ShortestDistance { get; private set; }

        public Vertex(int number)
        {
            Number = number;
        }

        public void SetExplored()
        {
            if (Explored)
                throw new InvalidOperationException(string.Format("Setting vertex {0} as explored, but it already is", Number));

            Explored = true;
        }

        public void SetShortestDistance(long distance)
        {
            ShortestDistance = distance;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})",
                                 Number,
                                 Explored ? "Explored" : "Unexplored",
                                 ShortestDistance);
        }
    }

    class Graph
    {
        private ICollection<Vertex> _vertices;
        private ICollection<WeightedEdge> _edges;

        Graph(ICollection<Vertex> vertices)
        {
            _vertices = vertices;
            _edges = new List<WeightedEdge>();
        }

        public void AddEdge(int labelFrom, int labelTo, long weight)
        {
            Vertex source = GetVertex(labelFrom);
            Vertex target = GetVertex(labelTo);
            WeightedEdge edge = new WeightedEdge(source, target, weight);

            _edges.Add(edge);
        }

        public Vertex GetVertex(int label)
        {
            var vertex = _vertices
                .Where(v => v.Number == label)
                .FirstOrDefault();

            if (vertex == null)
                throw new ArgumentOutOfRangeException("No vertex found with label " + label);

            return vertex;
        }

        public IEnumerable<WeightedEdge> EnumerableEdges()
        {
            return _edges;
        }

        public static Graph FromAdjacencyFile(string filename)
        {
            ICollection<Tuple<int, int, long>> edges = new List<Tuple<int, int, long>>();

            int maxNodeNumber = 0;

            using (TextReader reader = File.OpenText(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var nums = line.Split("\t");
                    int source = int.Parse(nums[0]);

                    for (int i = 1; i < nums.Length; i++)
                    {
                        if (nums[i].Length == 0)
                            continue;

                        var targetWeight = nums[i].Split(",");

                        int target = int.Parse(targetWeight[0]);
                        long weight = long.Parse(targetWeight[1]);

                        edges.Add(new Tuple<int, int, long>(source, target, weight));
                    }

                    if (source > maxNodeNumber)
                        maxNodeNumber = source;
                }
            }

            ICollection<Vertex> vertices = new List<Vertex>();

            for (int i = 1; i <= maxNodeNumber; i++)
                vertices.Add(new Vertex(i));

            Graph graph = new Graph(vertices);

            foreach (var edge in edges)
                graph.AddEdge(edge.Item1, edge.Item2, edge.Item3);

            return graph;
        }
    }
}