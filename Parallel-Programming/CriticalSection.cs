using System;
namespace Parallel_Programming
{
    public class CriticalSection
    {

        public CriticalSection()
        {
        }

        public void RunWithoutInterlock()
        {
            var bankAccount = new BankAccountWithLock();
            var tasks = new List<Task>();

            for (int i = 0; i < 10; i ++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bankAccount.Deposit(100);
                    }
                }));
            }

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bankAccount.Withdraw(100);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {bankAccount.Balance}");
        }

        public void RunWithInterlock()
        {
            var bankAccountInterlock = new BankAccountInterlock();
            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bankAccountInterlock.Deposit(100);
                    }
                }));
            }

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bankAccountInterlock.Withdraw(100);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {bankAccountInterlock.Balance}");
        }

        public void RunWithMutex()
        {
            var bankAccountWithMutex1 = new BankAccountWithMutex();
            var bankAccountWithMutex2 = new BankAccountWithMutex();
            var tasks = new List<Task>();

            Mutex mutex1 = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex1.WaitOne();
                        try
                        {
                            bankAccountWithMutex1.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock) mutex1.ReleaseMutex();
                        }
                    }
                }));
            }

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {
                            bankAccountWithMutex2.Deposit(1);
                        }
                        finally 
                        {
                            if (haveLock) mutex2.ReleaseMutex();
                        }
                        
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = WaitHandle.WaitAll(new[] { mutex1, mutex2 });
                        try
                        {
                            bankAccountWithMutex1.Transfer(bankAccountWithMutex2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex1.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance in 1 is {bankAccountWithMutex1.Balance}");
            Console.WriteLine($"Final balance in 2 is {bankAccountWithMutex2.Balance}");
        }

        public void RunSpinLock()
        {
            var bankAccount = new BankAccountWithLock();
            var tasks = new List<Task>();
            var spinLock = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool locked = false;
                        try
                        {
                            spinLock.Enter(ref locked);
                            bankAccount.Deposit(100);
                        }
                        finally
                        {
                            if (locked) spinLock.Exit();
                        }
                    }
                }));
            }

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool locked = false;
                        try
                        {
                            spinLock.Enter(ref locked);
                            bankAccount.Withdraw(100);
                        }
                        finally
                        {
                            if (locked) spinLock.Exit();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {bankAccount.Balance}");
        }

        /// <summary>
        /// This is DANGEROUS!
        /// You will get an exception after the first lock
        /// is grabbed because recursive functions do not
        /// handle locks well.
        /// </summary>
        /// <param name="x"></param>

        SpinLock spinLock = new SpinLock(true); //setting to true allows you to catch exceptions...
        public void RunRecursionSpinlock(int x)
        {
            bool locked = false;

            try
            {
                spinLock.Enter(ref locked);
            }
            catch (LockRecursionException e)
            {
                Console.WriteLine($"Exception {e}");
            }
            finally
            {
                if (locked)
                {
                    Console.WriteLine($"Grabbed lock with x = {x}");
                    RunRecursionSpinlock(x - 1);
                    spinLock.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock for {x}");
                }
            }
        }
    }
}

