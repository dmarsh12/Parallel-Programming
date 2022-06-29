using System;
namespace Parallel_Programming
{
    public class TaskCanceling
    {
        public TaskCanceling()
        {
        }

        public void Run()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            //if a token/task is canceled, this is called as it's subscribed to the token...
            token.Register(() =>
            {
                Console.WriteLine("Cancelation was requested.");
            });

            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    token.ThrowIfCancellationRequested(); //this is equivalent to the below code

                    /*
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        //break; you can break but it is recommended to throw operation canceled exception...
                        throw new OperationCanceledException();
                    }
                    */

                    Console.WriteLine($"{i++}\t");
                }
            }, token);

            t.Start();

            Task.Factory.StartNew(() =>
            {
                token.WaitHandle.WaitOne();
                Console.WriteLine("This is another way of handling cancelation events...");;
            });

            Console.ReadKey();
            cancellationTokenSource.Cancel();

            Console.WriteLine("Program finished");
        }

        public void RunParanoid()
        {
            //you can have multiple cancelation sources...
            var plannedCancelation = new CancellationTokenSource();
            var emergencyCancelation = new CancellationTokenSource();
            var preventativeCancelation = new CancellationTokenSource();

            var getAllCancelations = CancellationTokenSource.CreateLinkedTokenSource(
                plannedCancelation.Token, emergencyCancelation.Token, preventativeCancelation.Token);


            Task.Factory.StartNew(() =>
            {   
                int i = 0;
                while (true)
                {

                    getAllCancelations.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                    Thread.Sleep(1000);
                }
            }, getAllCancelations.Token);

            Console.ReadKey();

            //Any of the three below will cancel the task as they are a linked token source...
            emergencyCancelation.Cancel();
            //plannedCancelation.Cancel();
            //preventativeCancelation.Cancel();

            Console.WriteLine("Main program done");
            Console.ReadKey();
        }


    }
}

