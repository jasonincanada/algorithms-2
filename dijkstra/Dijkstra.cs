using System;
using System.Linq;

namespace jrh.Algorithms.Dijkstra
{
    class DijkstraSearch<T> where T : IEquatable<T>
    {
        private Graph<T> _graph;
        private ILogger _log;

        public DijkstraSearch(Graph<T> graph, Vertex<T> start, ILogger log)
        {
            _graph = graph;
            _log = log;

            start.SetExplored();
            start.SetShortestDistance(0);

            _log.Log("Starting search...");
            Search();
            _log.Log("Done search");
        }

        // Dijkstra's greedy score for an edge is the edge's weight plus the shortest distance
        // calculated up to the source vertex
        private static long GreedyScore(WeightedEdge<T> edge)
        {
            return edge.Source.ShortestDistance + edge.Weight;
        }

        // This is the naive O(mn) search algorithm, which is sufficient for our dataset.
        // Using a heap data structure instead of selecting over all edges in each
        // iteration would get it to O(m log n)
        private void Search()
        {
            var edges = _graph.EnumerableEdges();

            while (true)
            {
                var frontier = edges
                    .Where(edge => edge.Source.Explored && !edge.Target.Explored);

                // Select the "best" edge via Dijkstra's greedy score
                var next = frontier
                    .OrderBy(GreedyScore)
                    .FirstOrDefault();

                if (next == null)
                    break;

                next.Target.SetShortestDistance(GreedyScore(next));
                next.Target.SetExplored();
            }
        }
    }
}
