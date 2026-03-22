using Microsoft.ML.Data;

namespace FinTech.Models
{
    public class TransactionData
    {
        public float Amount { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
    }

    public class TransactionPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
} 