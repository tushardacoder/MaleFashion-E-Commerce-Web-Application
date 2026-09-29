using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Orders.Query;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Orders.Query
{
    
    public class GetOrderHistoryQueryHandlerTests
    {
        private AutoMock _mock;

        private GetOrderHistoryQueryHandler _handler;

        private Mock<IOrderRepository> _mockOrderRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mock =
                AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _mockOrderRepository =
                _mock.Mock<IOrderRepository>();

            _handler =
                _mock.Create<GetOrderHistoryQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockOrderRepository?.Reset();
        }

        [Test]
        public async Task Handle_UserHasOrders_ReturnsOrderHistory()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),

                    UserId = userId,

                    CreatedAt = DateTime.UtcNow,

                    Subtotal = 2000m,

                    DiscountAmount = 200m,

                    Total = 1800m
                },

                new Order
                {
                    Id = Guid.NewGuid(),

                    UserId = userId,

                    CreatedAt = DateTime.UtcNow.AddDays(-1),

                    Subtotal = 1500m,

                    DiscountAmount = 100m,

                    Total = 1400m
                }
            };

            _mockOrderRepository
                .Setup(x => x.GetByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);

            var query = new GetOrderHistoryQuery
            {
                UserId = userId
            };

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Orders.ShouldNotBeNull(),
                () => result.Orders.Count.ShouldBe(2),
                () => result.Orders[0].Id.ShouldBe(orders[0].Id),
                () => result.Orders[0].CreatedAt.ShouldBe(orders[0].CreatedAt),
                () => result.Orders[0].Subtotal.ShouldBe(orders[0].Subtotal),
                () => result.Orders[0].DiscountAmount.ShouldBe(orders[0].DiscountAmount),
                () => result.Orders[0].Total.ShouldBe(orders[0].Total),
                () => result.Orders[1].Id.ShouldBe(orders[1].Id),
                () => result.Orders[1].CreatedAt.ShouldBe(orders[1].CreatedAt),
                () => result.Orders[1].Subtotal.ShouldBe(orders[1].Subtotal),
                () => result.Orders[1].DiscountAmount.ShouldBe(orders[1].DiscountAmount),
                () => result.Orders[1].Total.ShouldBe(orders[1].Total),
                () => _mockOrderRepository.Verify(
                    x => x.GetByUserIdAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_UserHasNoOrders_ReturnsEmptyOrderHistory()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockOrderRepository
                .Setup(x => x.GetByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Order>());

            var query = new GetOrderHistoryQuery
            {
                UserId = userId
            };

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Orders.ShouldNotBeNull(),
                () => result.Orders.ShouldBeEmpty(),
                () => _mockOrderRepository.Verify(
                    x => x.GetByUserIdAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }
    }
}