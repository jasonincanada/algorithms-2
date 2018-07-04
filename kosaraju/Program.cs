/* Algorithms 2 - Graph Search, Shortest Paths, and Data Structures
   Stanford University via coursera.org

   Programming Assignment #1 - Find the strongly connected components of a directed graph

   Remarks:  This code works on arbitrary graph sizes, limited only by the computer's heap. It
             computes all strongly connected components of the input graph using Kosaraju's
             two-pass Depth-First Search algorithm.  My initial attempt to implement the
             algorithm using recursion failed with a stack overflow, so this version uses a
             stack of nodes along with their progress through all outgoing arrows in order
             to implement backtracking on the heap.

             The performance of this algorithm is blazingly fast (due to Kosaraju, not me!).
             The input graph consists of ~5.1 million edges, yet the finding of the SCCs takes
             only 1.4 SECONDS on my AMD 2.70 GHz processor.  It takes 10 seconds just to read
             and parse the input file!

             C:\Users\Jason\algorithms-2\kosaraju>dotnet run -c Release
             2018-07-04 18:44:50.831: Reading file...
             2018-07-04 18:45:00.220: DFS Pass 1...
             2018-07-04 18:45:00.891: DFS Pass 2...
             2018-07-04 18:45:01.622: Finding the largest 5 components...
             2018-07-04 18:45:01.643: Done

   Author: Jason Hooper
*/

using System;
using System.Linq;

namespace jrh.Algorithms.Kosaraju
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new ConsoleLogger();

            log.Log("Reading file...");
            Graph graph = Graph.FromFile("SCC.txt");

            Kosaraju kosaraju = new Kosaraju(graph, log);

            var components = kosaraju.FindSCCs();

            log.Log("Finding the largest 5 components...");
            var top5 = components
                .Select(kvp => kvp.Value.Count + 1)
                .OrderByDescending(n => n)                   
                .Take(5)
                .ToList();
            
            top5.ForEach(count => log.Log(count.ToString()));

            log.Log("Done");
        }        
    }   
}