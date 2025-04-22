using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Server.Data;
using PersonalFinanceManager.Server.Models;

namespace PersonalFinanceManager.Server.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<Transaction?> GetTransactionById(int id);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<Transaction?> UpdateTransactionAsync(int id, Transaction transaction);
        Task<bool> DeleteTransactionAsync(int id);
    }

    public class TransactionService : ITransactionService
    {
        private readonly FinanceManagerDbContext _context;

        public TransactionService(FinanceManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction?> GetTransactionById(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction?> UpdateTransactionAsync(int id, Transaction transaction)
        {
            var transactionToUpdate = await _context.Transactions.FindAsync(id);
            if (transactionToUpdate == null)
            {
                return null;
            }

            transactionToUpdate.Name = transaction.Name;
            transactionToUpdate.Description = transaction.Description;
            transactionToUpdate.Amount = transaction.Amount;
            transactionToUpdate.Date = transaction.Date;
            transactionToUpdate.CategoryId = transaction.CategoryId;
            //transactionToUpdate.Category = transaction.Category;

            await _context.SaveChangesAsync();

            return transactionToUpdate;
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transactionToDelete = await _context.Transactions.FindAsync(id);
            if (transactionToDelete == null)
            {
                return false;
            }

            _context.Transactions.Remove(transactionToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
