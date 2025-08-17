using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction.Domain.Entities
{
    public class TransactionEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = "Expense";
        public string UserId { get; set; } = string.Empty;
    }
}