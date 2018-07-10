/* Algorithms 2 - Graph Search, Shortest Paths, and Data Structures
   Stanford University via coursera.org

   Programming Assignment #3 - Median Maintenance
   
   Remarks:  This algorithm tracks the median values of a list of numbers as it grows. We
             use two heaps to represent the left and right halves of the list, keeping
             them balanced as numbers are added so the median is always the minimum of
             either the left or right heap.

   Author:  Jason Hooper
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace jrh.Algorithms.MedianMaintenance
{
    class Program
    {
        static void Main(string[] args)
        {
            ICollection<int> medians = new List<int>();

            MinHeap<int> rightHeap = new MinHeap<int>();

            // We'll be cheeky and use the negatives of numbers to simulate a max heap
            // (I saw this idea on the internet, it's not my own)
            MinHeap<int> leftHeap = new MinHeap<int>();

            // Example of storing 6 numbers in the two heaps: 4 8 12 | 16 24 28:
            //
            // leftHeap : -12 -8 -4
            // rightHeap: 16 24 28

            IEnumerable<int> numbers = NumbersFromFile("Median.txt");

            foreach (var number in numbers)
            {
                // Determine which heap this number is added to
                if (rightHeap.Size == 0)
                {
                    if (leftHeap.Size == 0 || number > -leftHeap.Min())
                        rightHeap.Add(number);
                    else
                        leftHeap.Add(-number);
                }
                else
                {
                    if (number >= rightHeap.Min())
                        rightHeap.Add(number);
                    else
                        leftHeap.Add(-number);
                }

                // Balance the heaps if necessary, by moving a number from one to the other
                int rightSize = rightHeap.Size;
                int leftSize = leftHeap.Size;

                if (rightSize - leftSize > 1)
                    leftHeap.Add(-rightHeap.ExtractMin());
                else if (leftSize - rightSize > 1)
                    rightHeap.Add(-leftHeap.ExtractMin());

                // Determine and collect the median of the numbers we've seen so far
                if (rightHeap.Size > leftHeap.Size)
                    medians.Add(rightHeap.Min());
                else
                    medians.Add(-leftHeap.Min());
            }

            Console.WriteLine("Sum of medians mod 10,000: {0}", medians.Sum() % 10000);
        }

        // Read one number per line
        static IEnumerable<int> NumbersFromFile(string filename)
        {
            using (TextReader reader = File.OpenText(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return int.Parse(line);
            }
        }
    }
}
