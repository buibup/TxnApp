using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Transaction.Application.Commands.CreateTransaction;
using Transaction.Application.Interfaces;
using Transaction.Domain.Entities;

namespace Transaction.Application.Handlers.CreateTransaction
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly ITransactionRepository _repo;

        public CreateTransactionHandler(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var txn = new TransactionEntity(
                Guid.NewGuid(),
                request.Description,
                request.Amount,
                request.Date,
                request.Type,
                request.UserId
            );

            await _repo.AddAsync(txn);
            return txn.Id;
        }
    }
}