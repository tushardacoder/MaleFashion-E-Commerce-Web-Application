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
    
     public class GetOrderDetailsQueryHandlerTests
    {
        private AutoMock _mock;

        private GetOrderDetailsQueryHandler _handler;

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
                _mock.Create<GetOrderDetailsQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockOrderRepository?.Reset();
        }

        [Test]
        public async Task Handle_OrderExistsAndBelongsToUser_ReturnsOrderDetails()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var userId = Guid.NewGuid();

            var order = new Order
            {
                Id = orderId,

                UserId = userId,

                FirstName = "Tushar",

                LastName = "Basak",

                Email = "tushar@example.com",

                Phone = "01700000000",

                Address = "Dhaka",

                TownCity = "Dhaka",

                CountryState = "Dhaka",

                PostcodeZip = "1207",

                OrderNotes = "Please deliver carefully",

                Subtotal = 2000m,

                DiscountAmount = 200m,

                Total = 1800m,

                CreatedAt = DateTime.UtcNow
            };

            _mockOrderRepository
                .Setup(x => x.GetByIdAsync(
                    orderId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var query = new GetOrderDetailsQuery
            {
                OrderId = orderId,

                UserId = userId
            };

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(order.Id),
                () => result.CreatedAt.ShouldBe(order.CreatedAt),
                () => result.FirstName.ShouldBe(order.FirstName),
                () => result.LastName.ShouldBe(order.LastName),
                () => result.Email.ShouldBe(order.Email),
                () => result.Phone.ShouldBe(order.Phone),
                () => result.Address.ShouldBe(order.Address),
                () => result.TownCity.ShouldBe(order.TownCity),
                () => result.CountryState.ShouldBe(order.CountryState),
                () => result.PostcodeZip.ShouldBe(order.PostcodeZip),
                () => result.OrderNotes.ShouldBe(order.OrderNotes),
                () => result.Subtotal.ShouldBe(order.Subtotal),
                () => result.DiscountAmount.ShouldBe(order.DiscountAmount),
                () => result.Total.ShouldBe(order.Total),
                () => _mockOrderRepository.Verify(
                    x => x.GetByIdAsync(
                        orderId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_OrderNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var userId = Guid.NewGuid();

            _mockOrderRepository
                .Setup(x => x.GetByIdAsync(
                    orderId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var query = new GetOrderDetailsQuery
            {
                OrderId = orderId,

                UserId = userId
            };

            // Act
            var exception = await Should.ThrowAsync<KeyNotFoundException>(
                () => _handler.Handle(
                    query,
                    CancellationToken.None));

            // Assert
            this.ShouldSatisfyAllConditions(
                () => exception.Message.ShouldBe("Order not found."),
                () => _mockOrderRepository.Verify(
                    x => x.GetByIdAsync(
                        orderId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_OrderBelongsToAnotherUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var orderUserId = Guid.NewGuid();

            var requestUserId = Guid.NewGuid();

            var order = new Order
            {
                Id = orderId,

                UserId = orderUserId,

                FirstName = "Tushar",

                LastName = "Basak",

                Email = "tushar@example.com",

                Subtotal = 2000m,

                Total = 2000m,

                CreatedAt = DateTime.UtcNow
            };

            _mockOrderRepository
                .Setup(x => x.GetByIdAsync(
                    orderId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var query = new GetOrderDetailsQuery
            {
                OrderId = orderId,

                UserId = requestUserId
            };

            // Act
            var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
                () => _handler.Handle(
                    query,
                    CancellationToken.None));

            // Assert
            this.ShouldSatisfyAllConditions(
                () => exception.Message.ShouldBe(
                    "You are not authorized to view this order."),
                () => _mockOrderRepository.Verify(
                    x => x.GetByIdAsync(
                        orderId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }
    }
}