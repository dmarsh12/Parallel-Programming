using System;
using System.Collections.Concurrent;

namespace Parallel_Programming
{
    public class ConcurrentDictionaries
    {
        public ConcurrentDictionaries()
        {
        }

        public ConcurrentDictionary<string, string> Capitals { get; set; }
            = new ConcurrentDictionary<string, string>();

        public void AddParis()
        {
            var result = Capitals.TryAdd("France", "Paris");
            string thread = Task.CurrentId.HasValue ? $"Task ID {Task.CurrentId}" : "Main Thread";
            Console.WriteLine($"Result {result} on thread {thread}");
        }

        public void Run()
        {
            Task.Factory.StartNew(() => AddParis());
            AddParis();

            Capitals["Russia"] = "Leningrad";
            Capitals.AddOrUpdate("Russia", "Moscow", (k, old) =>
            {
                return $"{old} --> Moscow ";
            });
        }
    }
}

