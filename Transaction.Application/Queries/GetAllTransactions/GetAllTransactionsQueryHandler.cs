using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Transaction.Application.Dtos;
using Transaction.Application.Interfaces;
using Transaction.Application.Mappers;

namespace Transaction.Application.Queries.GetAllTransactions
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
    {
        private readonly ITransactionRepository _transactionRepository;
        public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetAllAsync(request.UserId);
            return TransactionMapper.ToDtoList(transactions);
        }
    }
}