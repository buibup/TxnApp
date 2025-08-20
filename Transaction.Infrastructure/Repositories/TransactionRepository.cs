using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Transaction.Application.Interfaces;
using Transaction.Domain.Entities;
using Transaction.Infrastructure.Data;

namespace Transaction.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;
        public TransactionRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<TransactionEntity> AddAsync(TransactionEntity transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteAsync(Guid id, string userId)
        {
            var txn = _db.Transactions.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (txn is null) return false;

            _db.Transactions.Remove(txn);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<TransactionEntity>> GetAllAsync(string userId)
        {
            return await _db.Transactions
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<TransactionEntity?> GetByIdAsync(Guid id, string userId)
        {
            return await _db.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TransactionEntity?> UpdateAsync(TransactionEntity transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
            return transaction;
        }
    }
}