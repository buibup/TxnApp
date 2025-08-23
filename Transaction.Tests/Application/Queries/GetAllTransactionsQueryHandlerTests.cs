using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Transaction.Application.Dtos;
using Transaction.Application.Interfaces;
using Transaction.Application.Queries.GetAllTransactions;
using Transaction.Domain.Entities;

namespace Transaction.Tests.Application.Queries
{
    public class GetAllTransactionsQueryHandlerTests
    {
        private readonly Mock<ITransactionRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly GetAllTransactionsQueryHandler _handler;

        public GetAllTransactionsQueryHandlerTests()
        {
            _mockRepo = new Mock<ITransactionRepository>();

            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TransactionEntity, TransactionDto>();
            });

            _mapper = config.CreateMapper();

            _handler = new GetAllTransactionsQueryHandler(_mockRepo.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ReturnsListOfTransactionDto()
        {
            // Arrange
            var userId = "user-001";
            var transactions = new List<TransactionEntity>
            {
                new(
                    Guid.NewGuid(),
                    "Test 1",
                    100,
                    DateTime.UtcNow,
                    "Income",
                    userId
                ),
                new(
                    Guid.NewGuid(),
                    "Test 2",
                    200,
                    DateTime.UtcNow,
                    "Expense",
                    userId
                )
            };

            _mockRepo.Setup(repo => repo.GetAllAsync(userId)).ReturnsAsync(transactions);

            var query = new GetAllTransactionsQuery(userId);

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            result.Should().HaveCount(2);
            result[0].Description.Should().Be("Test 1");
            result[1].Amount.Should().Be(200);
        }
    }
}