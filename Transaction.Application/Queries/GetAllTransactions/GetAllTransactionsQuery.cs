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
        public string? Keyword { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public GetAllTransactionsQuery(string userId, string? keyword = null, int page = 1, int pageSize = 10)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            Keyword = keyword;
            Page = page;
            PageSize = pageSize;
        }
    }
}