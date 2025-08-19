using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Transaction.Application.EventAdapter
{
    public class TransactionCreatedHandler : INotificationHandler<TransactionCreatedEventNotification>
    {
        public Task Handle(TransactionCreatedEventNotification notification, CancellationToken cancellationToken)
        {
            var txn = notification.DomainEvent.Transaction;
            Console.WriteLine($"📢 Transaction Created: {txn.Description} - {txn.Amount} on {txn.Date}");
            return Task.CompletedTask;
        }
    }
}