/* Algorithms 2 - Graph Search, Shortest Paths, and Data Structures
   Stanford University via coursera.org

   Programming Assignment #1 - Find the strongly connected components of a directed graph

   Remarks:  This code works on arbitrary graph sizes, limited only by the computer's heap. It
             computes all strongly connected components of the input graph using Kosaraju's
             two-pass Depth-First Search algorithm.  My initial attempt using recursion failed
             with a stack overflow, so this version uses a stack of nodes along with their
             progress through all outgoing arrows in order to implement backtracking.

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