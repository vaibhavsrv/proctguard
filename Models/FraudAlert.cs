using System;

namespace FinTech.Models
{
    public class FraudAlert
    {
        public int Id { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime AlertTime { get; set; } = DateTime.UtcNow;
    }
} 