using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transaction.Application.Dtos;
using Transaction.Domain.Entities;

namespace Transaction.Application.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionDto ToDto(TransactionEntity txn)
        {
            return new TransactionDto
            {
                Id = txn.Id,
                Description = txn.Description,
                Amount = txn.Amount,
                Date = txn.Date,
                Type = txn.Type
            };
        }

        public static List<TransactionDto> ToDtoList(IEnumerable<TransactionEntity> txns)
        {
            return [.. txns.Select(ToDto)];
        }
    }
}