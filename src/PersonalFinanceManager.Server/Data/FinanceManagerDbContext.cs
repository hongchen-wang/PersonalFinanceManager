using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Server.Models;

namespace PersonalFinanceManager.Server.Data
{
    public class FinanceManagerDbContext : DbContext
    {
        public FinanceManagerDbContext(DbContextOptions options) : base(options)
        {
            // solution: https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        //public DbSet<Expense> Expenses { get; set; }
        //public DbSet<Income> Incomes { get; set; }
        //public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        //public DbSet<IncomeCategory> IncomeCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
