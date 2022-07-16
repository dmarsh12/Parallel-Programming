using System;
using System.Collections.Concurrent;

namespace Parallel_Programming
{
    /// <summary>
    /// These are unordered but maintain the appropriate values
    /// at each index
    /// </summary>
    public class ConcurrentBags
    {
        public ConcurrentBags()
        {
        }

        public void Run()
        {
            var bag = new ConcurrentBag<int>();
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var i1 = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    bag.Add(i1);
                    Console.WriteLine($"Task {Task.CurrentId} has value {i1}");
                    int result;
                    if (bag.TryPeek(out result))
                    {
                        Console.WriteLine($"{Task.CurrentId} has peeked the value of {result}");
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}

