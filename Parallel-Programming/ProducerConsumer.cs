using System;
using System.Collections.Concurrent;

namespace Parallel_Programming
{
    public class ProducerConsumer
    {
        public ProducerConsumer()
        {
        }

        BlockingCollection<int> messages = new BlockingCollection<int>(new ConcurrentBag<int>(), 10);

        CancellationTokenSource cts = new CancellationTokenSource();

        Random random = new Random();

        public void Run()
        {
            Task.Factory.StartNew(ProduceAndConsume, cts.Token);

            Console.ReadKey();

            cts.Cancel();
        }

        public void ProduceAndConsume()
        {
            var producer = Task.Factory.StartNew(RunProducer);
            var consumer = Task.Factory.StartNew(RunConsumer);

            try
            {
                Task.WaitAll(new[] { producer, consumer }, cts.Token);
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => true);
            }

        }

        public void RunProducer()
        {
            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();
                int x = random.Next(100);
                messages.Add(x);
                Console.WriteLine($"+{x}\t");
                Thread.Sleep(random.Next(1000));
            }
        }

        public void RunConsumer()
        {
            foreach (var i in messages.GetConsumingEnumerable())
            {
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"-{i}\t");
                Thread.Sleep(random.Next(1000));
            }
        }
    }
}

