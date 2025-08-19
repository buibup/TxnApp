using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Transaction.Application.Common;
using Transaction.Domain.Common;
using Transaction.Domain.Entities;

namespace Transaction.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        public DbSet<TransactionEntity> Transactions { get; set; }

        public AppDbContext(DbContextOptions options, IDomainEventDispatcher dispatcher)
            : base(options) => _dispatcher = dispatcher;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var entitiesWithEvents = ChangeTracker
                .Entries<Entity<Guid>>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity);

            await _dispatcher.DispatchEventsAsync(entitiesWithEvents);

            return result;
        }
    }
}