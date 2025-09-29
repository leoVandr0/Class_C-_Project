using System;
using SmartEco.Models;

namespace SmartEco.Services
{
    public class AccountService
    {
        private decimal balance;

        public AccountService(decimal initialBalance = 1000m)
        {
            balance = initialBalance;
        }

        public decimal GetBalance() => balance;

        public bool TryPurchase(Transaction transaction, out string message)
        {
            if (transaction.Amount > balance)
            {
                message = "Insufficient balance.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(transaction.Phone) || transaction.Phone.Length < 9)
            {
                message = "Invalid phone number.";
                return false;
            }

            balance -= transaction.Amount;
            message = $"Success: {transaction}";
            return true;
        }
    }
}
