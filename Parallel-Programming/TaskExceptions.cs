using System;
namespace Parallel_Programming
{
    public class TaskExceptions
    {
        public TaskExceptions()
        {
        }

        public void Run()
        {
            try
            {
                HandleExceptions();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine($"Handled exceptions {e.GetType()}");
                }
            }

            Console.WriteLine("Main program completed");
            Console.ReadKey();
        }

        private static void HandleExceptions()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                throw new InvalidOperationException("Operation not allowed") { Source = "t1" };
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                throw new AccessViolationException("Not accessible") { Source = "t1" };
            });

            try
            {
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ae)
            {
                //foreach (var e in ae.InnerExceptions)
                //{
                //    Console.WriteLine($"Excpetion {e.GetType()} from {e.Source}");
                //}

                ae.Handle(e =>
                {
                    if (e is InvalidOperationException)
                    {
                        Console.WriteLine("Invalid operation!");
                        return true;
                    }
                    return false;
                });
            }
        }
    }
}

