using System.Collections.Generic;
using System.IO;
using SmartEco.Models;

namespace SmartEco.Services
{
    public class HistoryService
    {
        private List<Transaction> transactions = new List<Transaction>();

        public void Add(Transaction transaction)
        {
            transactions.Add(transaction);
        }

        public List<Transaction> GetAll()
        {
            return new List<Transaction>(transactions);
        }

        public void Clear()
        {
            transactions.Clear();
        }

        public void SaveToFile(string filePath = "transaction_history.txt")
        {
            var lines = new List<string>();
            foreach (var tx in transactions)
                lines.Add(tx.ToString());

            File.WriteAllLines(filePath, lines);
        }
    }
}
