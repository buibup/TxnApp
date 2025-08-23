using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Transaction.Application.Dtos;
using Transaction.Domain.Entities;

namespace Transaction.Application.Mapping
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionEntity, TransactionDto>().ReverseMap();
        }
    }
}