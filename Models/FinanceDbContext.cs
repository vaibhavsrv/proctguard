using Microsoft.EntityFrameworkCore;

namespace FinTech.Models
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<FraudAlert> FraudAlerts { get; set; }
    }
} 