using System;

namespace jrh.Algorithms.Kosaraju
{
    class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff}: {1}",
                              DateTime.UtcNow,
                              message);
        }
    }
}