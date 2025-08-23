using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Transaction.Application.Dtos;
using Transaction.Application.Interfaces;
using Transaction.Application.Mappers;
using AutoMapper;

namespace Transaction.Application.Queries.GetAllTransactions
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var all = await _transactionRepository.GetAllAsync(request.UserId);

            var filtered = string.IsNullOrEmpty(request.Keyword)
                ? all
                : all.Where(t => t.Description.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            var paged = filtered
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return _mapper.Map<List<TransactionDto>>(paged);
        }
    }
}