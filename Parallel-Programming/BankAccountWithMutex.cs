using System;
namespace Parallel_Programming
{
    public class BankAccountWithMutex
    {
        public BankAccountWithMutex()
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

        public void Transfer(BankAccountWithMutex bankAccount, int amount)
        {
            Balance -= amount;
            bankAccount.Deposit(amount);
        }
    }
}

