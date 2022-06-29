using System;
namespace Parallel_Programming
{
    public class TaskTime
    {
        public TaskTime()
        {
        }

        public void DisarmBombTask()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Task.Factory.StartNew(() =>
            {
                // Thread.Sleep(1000); // pauses thread and allows you to grab another thread...
                // SpinWait.SpinUntil(); // pauses thread but don't give up place in execution... prevents context switching
                Console.WriteLine("Disarm the bomb; you have 5 seconds!");
                bool canceled = token.WaitHandle.WaitOne(5000);
                Console.WriteLine(canceled ? "Bomb disarmed!" : "BOOM!");
            }, token);

            Console.ReadKey();
            cts.Cancel();

            Console.WriteLine("Main program done");
            Console.ReadKey();
        }

        public void WaitingTask()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var taskOne = new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Task one completed");

            }, token);

            var taskTwo = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("Task two completed");
            }, token);

            taskOne.Start();

            //Canceling a task while waiting for it to complete throws exception..
            //Console.ReadKey();
            //cts.Cancel();

            //taskOne.Wait(token);
            //Task.WaitAll(taskOne, taskTwo);
            //Task.WaitAny(taskOne, taskTwo); //stop tasks if one is completed
            Task.WaitAll(new[] { taskOne, taskTwo }, 4000, token); //tasks with a timeout

            Console.WriteLine($"Task one status: {taskOne.Status}");
            Console.WriteLine($"Task two status: {taskTwo.Status}");

            Console.WriteLine("Program Terminated");
            Console.ReadKey();
        }
        
    }
}

