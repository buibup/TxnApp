using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transaction.Domain.Common;

namespace Transaction.Application.Common
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(IEnumerable<Entity<Guid>> entities);
    }
}