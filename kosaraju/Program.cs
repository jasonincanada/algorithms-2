/* Algorithms 2 - Graph Search, Shortest Paths, and Data Structures
   Stanford University via coursera.org

   Programming Assignment #1 - Find the strongly connected components of a directed graph

   Remarks:  This code works on small sample sizes. It correctly computes the 3 strongly connected
             components of the graph listed in Sample.txt, which is the 9-node graph shown on the
             lecture slides.  With the main assignment input though it overflows the stack around
             the 36,000th vertex, and there are ~875,000 vertices.  So the next version will need
             to use a stack of nodes to backtrack instead of recursively calling DFS().

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
            Console.WriteLine("Reading file...");
            Graph graph = Graph.FromFile("Sample.txt");

            Kosaraju kosaraju = new Kosaraju(graph);

            var components = kosaraju.FindSCCs();

            Console.WriteLine("Finding the largest 5 components...");
            var top5 = components
                .Select(kvp => kvp.Value.Count + 1)
                .OrderByDescending(n => n)                   
                .Take(5)
                .ToList();
            
            top5.ForEach(count => Console.WriteLine("{0}", count));
        }        
    }   
}