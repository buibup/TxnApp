using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Transaction.Application.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<Guid>
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = "Expense";
        public string UserId { get; set; } = string.Empty;
    }
}