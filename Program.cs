using FinTech.Hubs;
using FinTech.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ML.NET model registration
var mlContext = new MLContext();
ITransformer mlModel;
try
{
    mlModel = mlContext.Model.Load("fraudModel.zip", out var modelInputSchema);
    var predEngine = mlContext.Model.CreatePredictionEngine<TransactionData, TransactionPrediction>(mlModel);
    builder.Services.AddSingleton(predEngine);
    Console.WriteLine("ML.NET model loaded successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading ML.NET model: {ex.Message}");
    // Create a dummy prediction engine that always returns false (no fraud)
    var dummyModel = mlContext.Model.Load("fraudModel.zip", out var dummySchema);
    var dummyEngine = mlContext.Model.CreatePredictionEngine<TransactionData, TransactionPrediction>(dummyModel);
    builder.Services.AddSingleton(dummyEngine);
}

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FraudAlertHub>("/fraudAlertHub");

app.Run();
