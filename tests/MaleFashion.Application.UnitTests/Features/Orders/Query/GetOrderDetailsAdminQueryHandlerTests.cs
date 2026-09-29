using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
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
   public class GetOrderDetailsAdminQueryHandlerTests
    {
        private AutoMock _mock;

        private GetOrderDetailsAdminQueryHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

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
            _mockUnitOfWork =
                _mock.Mock<IApplicationUnitOfWork>();

            _mockOrderRepository =
                _mock.Mock<IOrderRepository>();

            _handler =
                _mock.Create<GetOrderDetailsAdminQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockOrderRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }

        [Test]
        public async Task Handle_OrderExists_ReturnsOrderDetails()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var order = new Order
            {
                Id = orderId,
                UserId = Guid.NewGuid(),
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

            _mockUnitOfWork
                .SetupGet(x => x.OrderRepository)
                .Returns(_mockOrderRepository.Object);

            _mockOrderRepository
                .Setup(x => x.GetOrderDetailsAsync(
                    orderId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var query = new GetOrderDetailsAdminQuery
            {
                OrderId = orderId
            };

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(order.Id),
                () => result.UserId.ShouldBe(order.UserId),
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
                () => result.CreatedAt.ShouldBe(order.CreatedAt),
                () => _mockOrderRepository.Verify(
                    x => x.GetOrderDetailsAsync(
                        orderId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_OrderNotFound_ReturnsNull()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var query = new GetOrderDetailsAdminQuery
            {
                OrderId = orderId
            };

            _mockUnitOfWork
                .SetupGet(x => x.OrderRepository)
                .Returns(_mockOrderRepository.Object);

            _mockOrderRepository
                .Setup(x => x.GetOrderDetailsAsync(
                    orderId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),
                () => _mockOrderRepository.Verify(
                    x => x.GetOrderDetailsAsync(
                        orderId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }
    }
}