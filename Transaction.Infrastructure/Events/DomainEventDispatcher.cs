using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Transaction.Application.Common;
using Transaction.Application.EventAdapter;
using Transaction.Domain.Common;
using Transaction.Domain.Events;

namespace Transaction.Infrastructure.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchEventsAsync(IEnumerable<Entity<Guid>> entities)
        {
            foreach (var entity in entities)
            {
                foreach (var domainEvent in entity.DomainEvents)
                {
                    switch (domainEvent)
                    {
                        case TransactionCreatedEvent created:
                            await _mediator.Publish(new TransactionCreatedEventNotification(created));
                            break;
                            // ✅ เพิ่ม event อื่น ๆ ตรงนี้ได้
                    }
                }

                entity.ClearDomainEvents();
            }
        }
    }
}