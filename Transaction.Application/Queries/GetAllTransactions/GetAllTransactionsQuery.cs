using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Transaction.Application.Dtos;

namespace Transaction.Application.Queries.GetAllTransactions
{
    public class GetAllTransactionsQuery : IRequest<List<TransactionDto>>
    {
        public string UserId { get; }

        public GetAllTransactionsQuery(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }
    }
}