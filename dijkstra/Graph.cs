using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace jrh.Algorithms.Dijkstra
{
    class WeightedEdge<T>
    {
        public Vertex<T> Source { get; private set; }
        public Vertex<T> Target { get; private set; }
        public long Weight { get; private set; }

        public WeightedEdge(Vertex<T> source, Vertex<T> target, long weight)
        {
            Source = source;
            Target = target;
            Weight = weight;
        }

        public override string ToString()
        {
            return string.Format("{0} --{1}--> {2}",
                                 Source.ToString(),
                                 Weight,
                                 Target.ToString());
        }
    }

    class Vertex<T>
    {
        public T Obj { get; private set; }
        public bool Explored { get; private set; }
        public long ShortestDistance { get; private set; }

        public Vertex(T obj)
        {
            Obj = obj;
        }

        public void SetExplored()
        {
            if (Explored)
                throw new InvalidOperationException(string.Format("Setting vertex {0} as explored, but it already is", Obj.ToString()));

            Explored = true;
        }

        public void SetShortestDistance(long distance)
        {
            ShortestDistance = distance;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})",
                                 Obj.ToString(),
                                 Explored ? "Explored" : "Unexplored",
                                 ShortestDistance);
        }
    }

    class Graph<T> where T : IEquatable<T>
    {
        private ICollection<Vertex<T>> _vertices;
        private ICollection<WeightedEdge<T>> _edges;

        Graph(ICollection<Vertex<T>> vertices)
        {
            _vertices = vertices;
            _edges = new List<WeightedEdge<T>>();
        }

        public void AddEdge(T from, T to, long weight)
        {
            Vertex<T> source = GetVertex(from);
            Vertex<T> target = GetVertex(to);
            WeightedEdge<T> edge = new WeightedEdge<T>(source, target, weight);

            _edges.Add(edge);
        }

        public Vertex<T> GetVertex(T target)
        {
            var vertex = _vertices
                .Where(v => v.Obj.Equals(target))
                .FirstOrDefault();

            if (vertex == null)
                throw new ArgumentOutOfRangeException("Could not find vertex " + target.ToString());

            return vertex;
        }

        public IEnumerable<WeightedEdge<T>> EnumerableEdges()
        {
            return _edges;
        }

        public static Graph<int> IntsFromAdjacencyFile(string filename)
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

            ICollection<Vertex<int>> vertices = new List<Vertex<int>>();

            for (int i = 1; i <= maxNodeNumber; i++)
                vertices.Add(new Vertex<int>(i));

            Graph<int> graph = new Graph<int>(vertices);

            foreach (var edge in edges)
                graph.AddEdge(edge.Item1, edge.Item2, edge.Item3);

            return graph;
        }
    }
}