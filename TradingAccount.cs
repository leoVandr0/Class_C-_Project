using System;
using System.ComponentModel.DataAnnotations;

namespace SmartEco.Models
{
    public class TradingAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        public decimal Balance { get; set; } = 0m;

        [StringLength(3)]
        public string Currency { get; set; } = "USD";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual User User { get; set; }
    }
}