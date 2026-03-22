# 🛡️ Real-Time Fraud Detection

> Submit a transaction. Get an instant verdict. Powered by ML.NET and live WebSocket alerts.

[![.NET](https://img.shields.io/badge/.NET_8-ASP.NET_Core-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)
[![ML.NET](https://img.shields.io/badge/ML.NET-Fraud_Model-DD2C00?style=flat-square&logo=microsoft)](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)
[![SignalR](https://img.shields.io/badge/SignalR-Real--Time_Alerts-00C4B4?style=flat-square&logo=microsoftazure)](https://learn.microsoft.com/aspnet/signalr)
[![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?style=flat-square&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)

---

## What It Does

A FinTech web application that scores financial transactions for fraud **in real time** — using a pre-trained ML.NET model and pushing live alerts to every connected client via SignalR the moment fraud is detected.

No page refresh. No polling. Just instant verdicts.

---

## How It Works

```
POST /api/transaction
        │
        ▼
  ML.NET model scores transaction
  (Amount + AccountNumber → Probability)
        │
   ┌────┴────┐
   │         │
  SAFE     FRAUD
   │         │
  200 OK   400 + SignalR broadcast
            → all clients notified instantly
```

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend framework | ASP.NET Core (.NET 8) |
| Fraud model | ML.NET (`fraudModel.zip`) |
| Real-time alerts | SignalR WebSockets |
| Database | SQL Server + Entity Framework Core |
| Frontend | Vanilla HTML/JS (served as static files) |

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server or SQL Server Express / LocalDB

### Setup

```bash
# 1. Clone the repo
git clone https://github.com/jidhinp/Real-Time-Fraud-Detection.git
cd Real-Time-Fraud-Detection

# 2. Set your SQL Server connection string in appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=FinanceDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

# 3. Apply migrations
dotnet ef database update

# 4. Run
dotnet run
```

Then open **http://localhost:5276** in your browser.

---

## API

### `POST /api/transaction`

Submit a transaction for fraud scoring.

**Request body:**
```json
{
  "transactionId": "TXN-001",
  "amount": 9500.00,
  "accountNumber": "ACC-4821"
}
```

**Responses:**

| Status | Meaning |
|---|---|
| `200 OK` | Transaction is clean |
| `400 Bad Request` | Fraud detected — includes ML probability score |

**Fraud response example:**
```json
{
  "message": "Fraud detected",
  "reason": "ML model flagged as fraud (probability: 94%)"
}
```

---

## Real-Time Alerts

When a transaction is flagged, all connected clients instantly receive a **SignalR push** via the `/fraudAlertHub` endpoint — no refresh needed.

```js
connection.on("ReceiveFraudAlert", (transactionId, reason) => {
  // Show alert in UI immediately
});
```

---

## ML Model

The pre-trained `fraudModel.zip` (ML.NET) scores each transaction on:

| Input | Type |
|---|---|
| `Amount` | float |
| `AccountNumber` | string |

| Output | Type |
|---|---|
| `PredictedLabel` | bool — fraud or not |
| `Probability` | float — confidence score |
| `Score` | float — raw model score |

> The model makes every decision — no hardcoded rules.

---

## Project Structure

```
├── Controllers/
│   └── TransactionController.cs   # API + ML prediction + SignalR trigger
├── Hubs/
│   └── FraudAlertHub.cs           # SignalR WebSocket hub
├── Models/
│   ├── Transaction.cs             # DB entity
│   ├── FraudAlert.cs              # Alert entity
│   ├── MLModels.cs                # ML input/output classes
│   ├── TransactionDto.cs          # API request DTO
│   └── FinanceDbContext.cs        # EF Core context
├── wwwroot/
│   └── index.html                 # Demo UI
├── fraudModel.zip                 # Pre-trained ML.NET model
└── Program.cs                     # App setup + ML model registration
```

---

## What's Next

- [ ] Enrich model features — add timestamp, location, velocity signals
- [ ] Retrain model on a larger labeled dataset (e.g. IEEE-CIS)
- [ ] Add transaction history view in the UI
- [ ] Role-based auth for analyst vs. customer access
- [ ] Deploy to Azure App Service + Azure SQL

---

## License

Educational / demo project. Adapt freely for your own use.