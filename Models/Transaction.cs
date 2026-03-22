using System;

namespace FinTech.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsFraud { get; set; }
    }
} 