using Microsoft.AspNetCore.SignalR;

namespace FinTech.Hubs
{
    public class FraudAlertHub : Hub
    {
        // Send a fraud alert to all connected clients
        public async Task SendFraudAlert(string transactionId, string reason)
        {
            await Clients.All.SendAsync("ReceiveFraudAlert", transactionId, reason);
        }
    }
} 