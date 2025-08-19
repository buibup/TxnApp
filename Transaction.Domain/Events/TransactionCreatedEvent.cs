using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transaction.Domain.Entities;

namespace Transaction.Domain.Events
{
    public class TransactionCreatedEvent : IDomainEvent
{
    public TransactionEntity Transaction { get; }

    public TransactionCreatedEvent(TransactionEntity txn)
    {
        Transaction = txn;
    }
}
}