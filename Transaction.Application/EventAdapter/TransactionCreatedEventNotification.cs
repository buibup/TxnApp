using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Transaction.Domain.Events;

namespace Transaction.Application.EventAdapter
{
    public class TransactionCreatedEventNotification : INotification
{
    public TransactionCreatedEvent DomainEvent { get; }

    public TransactionCreatedEventNotification(TransactionCreatedEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}
}