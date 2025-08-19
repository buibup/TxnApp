using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transaction.Domain.Entities;

namespace Transaction.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<TransactionEntity>> GetAllAsync(string userId);
        Task<TransactionEntity?> GetByIdAsync(Guid id, string userId);
        Task<TransactionEntity> AddAsync(TransactionEntity transaction);
        Task<TransactionEntity?> UpdateAsync(TransactionEntity transaction);
        Task<bool> DeleteAsync(Guid id, string userId);
    }
}