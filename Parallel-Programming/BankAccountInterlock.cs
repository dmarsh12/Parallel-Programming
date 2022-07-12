using System;
namespace Parallel_Programming
{
    public class BankAccountInterlock
    {
        private int balance;

        public BankAccountInterlock()
        {
        }

        public int Balance { get => balance; private set => balance = value; }

        public void Deposit(int amount)
        {
            Interlocked.Add(ref balance, amount);
        }

        public void Withdraw(int amount)
        {
            Interlocked.Add(ref balance, -amount);
        }
    }
}

