/* Algorithms 2 - Graph Search, Shortest Paths, and Data Structures
   Stanford University via coursera.org

   Programming Assignment #2 - Dijkstra's shortest path algorithm

   Remarks:  This is the naive O(mn) implementation of Dijkstra's shortest path algorithm.
             The input graph size is small enough that this processes in less than a second,
             but a better implementation would use a heap data structure that worked more
             efficiently with frontier edges.

             C:\Users\Jason\algorithms-2\dijkstra>dotnet run -c Release
             2018-07-08 10:45:49.736: Starting search...
             2018-07-08 10:45:49.767: Done search

   Author: Jason Hooper
*/

using System;
using System.Linq;

namespace jrh.Algorithms.Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = Graph<int>.IntsFromAdjacencyFile("sample.txt");
            Vertex<int> start = graph.GetVertex(1);

            var search = new DijkstraSearch<int>(graph,
                                                 start,
                                                 new ConsoleLogger());

            var inspect = new int[] { 1, 2, 3, 4 };

            foreach (var i in inspect)
                Console.WriteLine("{0}: {1}", i, graph.GetVertex(i).ShortestDistance);
        }
    }
}
