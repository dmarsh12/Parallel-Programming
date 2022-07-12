using System;
namespace Parallel_Programming
{
    public class BankAccountWithLock
    {
        private readonly object _lock = new object();

        public BankAccountWithLock()
        {
        }

        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            lock (_lock)
            {
                Balance += amount;
            }
        }

        public void Withdraw(int amount)
        {
            lock (_lock)
            {
                Balance -= amount;
            }
        }
    }
}

