namespace SmartEco.Models
{
    public class Transaction
    {
        public string Type { get; set; }  // Airtime or Data
        public string Phone { get; set; }
        public string Detail { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"[{Type}] {Detail} for {Phone} (${Amount})";
        }
    }
}
