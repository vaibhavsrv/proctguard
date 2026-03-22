using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.ML;
using FinTech.Hubs;
using FinTech.Models;

namespace FinTech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IHubContext<FraudAlertHub> _hubContext;
        private readonly FinanceDbContext _db;
        private readonly PredictionEngine<TransactionData, TransactionPrediction> _predEngine;

        public TransactionController(
            IHubContext<FraudAlertHub> hubContext,
            FinanceDbContext db,
            PredictionEngine<TransactionData, TransactionPrediction> predEngine)
        {
            _hubContext = hubContext;
            _db = db;
            _predEngine = predEngine;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionDto transaction)
        {
            var txn = new Transaction
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                AccountNumber = transaction.AccountNumber
            };

            // ML.NET prediction
            var input = new TransactionData
            {
                Amount = (float)transaction.Amount,
                AccountNumber = transaction.AccountNumber
            };
            var prediction = _predEngine.Predict(input);

            bool isFraud = prediction.PredictedLabel;
            txn.IsFraud = isFraud;
            
            // Debug output
            Console.WriteLine($"Transaction: Amount={transaction.Amount}, Account={transaction.AccountNumber}");
            Console.WriteLine($"Prediction: IsFraud={isFraud}, Probability={prediction.Probability:P2}");
            
            // Let the AI model make the decision - no hardcoded rules!
            _db.Transactions.Add(txn);

            if (isFraud)
            {
                string reason = $"ML model flagged as fraud (probability: {prediction.Probability:P0})";
                var alert = new FraudAlert { TransactionId = txn.TransactionId, Reason = reason };
                _db.FraudAlerts.Add(alert);
                await _hubContext.Clients.All.SendAsync("ReceiveFraudAlert", txn.TransactionId, reason);
                await _db.SaveChangesAsync();
                return BadRequest(new { message = "Fraud detected", reason });
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Transaction successful" });
        }
    }
} 