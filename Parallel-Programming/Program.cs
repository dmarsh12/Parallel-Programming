// See https://aka.ms/new-console-template for more information
using Parallel_Programming;

#region Task Basics
//var taskProgramming = new TaskBasics();
//taskProgramming.Run();
#endregion

#region Canceling Tasks
//var taskCancelling = new TaskCanceling();
//taskCancelling.Run();
//taskCancelling.RunParanoid();
#endregion

#region Task Waiting
//var taskTime = new TaskTime();
//taskTime.TaskThatCanBeCanceled();
//taskTime.WaitingTask();
#endregion

#region Exceptions
//var taskExceptions = new TaskExceptions();
//taskExceptions.Run();
#endregion

#region Critical Section
//var criticalSection = new CriticalSection();
//criticalSection.RunWithoutInterlock();
//criticalSection.RunWithInterlock();
//criticalSection.RunSpinLock();
//criticalSection.RunRecursionSpinlock(5);
//criticalSection.RunWithMutex();
#endregion

#region Concurrent Collections
//var concurrentCollections = new ConcurrentCollections();
#endregion

#region Producer Consumer
//var producerConsumer = new ProducerConsumer();
//producerConsumer.Run();
#endregion

#region AsyncAwait
var asyncAwait = new AsyncAwait();
await asyncAwait.Run();
#endregion