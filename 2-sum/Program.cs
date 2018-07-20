/* Algorithms 2 - Graph Search, Shortest Paths, and Data Structures
   Stanford University via coursera.org

   Programming Assignment #4 - Variation of the 2-Sum algorithm

   Remarks:  This algorithm counts the number of pairs of distinct integers in a
             list which sum to a value in the interval [-10,000, +10,000].  This
             week's lectures are about hash tables and Bloom filters, but we
             won't be fooled: this can be implemented much more efficiently by
             sorting and then smartly traversing the list.

             C:\Users\Jason\algorithms-2\2-sum>dotnet run -c Release
             2018-07-20 00:31:51.670: Reading numbers...
             2018-07-20 00:31:52.081: Starting loop...
             2018-07-20 00:31:52.090: Done

   Author: Jason Hooper
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace jrh.Algorithms.TwoSum
{
    class Program
    {
        static void Main(string[] args)
        {
            // Track which sums we've seen
            bool[] sums = new bool[20001];

            var nums = NumbersFromFile("2sum-sorted.txt");

            int i = 0;
            int j = nums.Count - 1;

            log.Log("Starting loop...");

            while (i < j)
            {
                long a = nums[i];
                long b = nums[j];
                long sum = a + b;

                if (sum > 10000)
                {
                    j--;
                    continue;
                }

                if (sum < -10000)
                {
                    i++;
                    continue;
                }

                int k = j;
                while (k > i)
                {
                    b = nums[k];
                    sum = a + b;

                    if (sum < -10000)
                        break;

                    // If we're here, we've found a sum between -10,000 and 10,000.
                    // If the summands are distinct, set this sum as seen
                    if (a != b)
                        sums[sum + 10000] = true;

                    k--;
                }

                i++;
            }

            log.Log("Done");
            log.Log($"Number of sums seen: {sums.Where(b => b).Count()}");
        }

        // Read one number per line
        static List<long> NumbersFromFile(string filename)
        {
            var nums = new List<long>();

            log.Log("Reading numbers...");
            using (TextReader reader = File.OpenText(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    nums.Add(long.Parse(line));
            }

            return nums;
        }

        private static ILogger log = new ConsoleLogger();
    }
}
