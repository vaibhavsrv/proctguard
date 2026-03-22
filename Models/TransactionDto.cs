namespace FinTech.Models
{
    public class TransactionDto
    {
        public string TransactionId { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
    }
} 