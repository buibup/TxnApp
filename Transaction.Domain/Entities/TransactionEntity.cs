using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transaction.Domain.Common;
using Transaction.Domain.Events;

namespace Transaction.Domain.Entities
{
    public class TransactionEntity : Entity<Guid>
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = "Expense"; // "Income" or "Expense"
        public string UserId { get; set; } = string.Empty;

        private TransactionEntity() { }

        public TransactionEntity(Guid guid, string description, decimal amount, DateTime date, string type, string userId)
        {
            Id = guid;
            Description = description;
            Amount = amount;
            Date = date;
            Type = type;
            UserId = userId;

            AddDomainEvent(new TransactionCreatedEvent(this));
        }

        public void Update(string description, decimal amount, DateTime date, string type)
        {
            Description = description;
            Amount = amount;
            Date = date;
            Type = type;
        }
    }
}