using System;
namespace Parallel_Programming
{
    public class BankAccountReaderWriterLock
    {
        public BankAccountReaderWriterLock()
        {
        }

        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }
    }
}

