using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Orders.Command;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Orders.Comamnd
{
    public class AddOrderCommandHandlerTests
    {
        private AutoMock _mock;

        private AddOrderCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<ICartRepository> _mockCartRepository;

        private Mock<IDiscountRepository> _mockDiscountRepository;

        private Mock<IInventoryRepository> _mockInventoryRepository;

        private Mock<IOrderRepository> _mockOrderRepository;

        private Mock<ITransactionManager> _mockTransactionManager;


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

            _mockCartRepository =
                _mock.Mock<ICartRepository>();

            _mockDiscountRepository =
                _mock.Mock<IDiscountRepository>();

            _mockInventoryRepository =
                _mock.Mock<IInventoryRepository>();

            _mockOrderRepository =
                _mock.Mock<IOrderRepository>();

            _mockTransactionManager =
                _mock.Mock<ITransactionManager>();

            _handler =
                _mock.Create<AddOrderCommandHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockTransactionManager?.Reset();

            _mockOrderRepository?.Reset();

            _mockInventoryRepository?.Reset();

            _mockDiscountRepository?.Reset();

            _mockCartRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }



        // ============================================================
        // TEST 1: User ID is missing
        // ============================================================

        [Test]
        public async Task Handle_UserIdMissing_ReturnsEmptyGuid()
        {
            // Arrange
            var command = new AddOrderCommand
            {
                UserId = null
            };

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(Guid.Empty),

                () => _mockCartRepository.Verify(
                    x => x.GetByUserIdWithItemsAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockTransactionManager.Verify(
                    x => x.BeginTransactionAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }



        // ============================================================
        // TEST 2: Cart not found
        // ============================================================

        [Test]
        public async Task Handle_CartNotFound_ReturnsEmptyGuid()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var command = new AddOrderCommand
            {
                UserId = userId
            };

            _mockUnitOfWork
                .SetupGet(x => x.CartRepository)
                .Returns(_mockCartRepository.Object);

            _mockCartRepository
                .Setup(x => x.GetByUserIdWithItemsAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(Guid.Empty),

                () => _mockCartRepository.Verify(
                    x => x.GetByUserIdWithItemsAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockTransactionManager.Verify(
                    x => x.BeginTransactionAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }



        // ============================================================
        // TEST 3: Cart is empty
        // ============================================================

        [Test]
        public async Task Handle_EmptyCart_ReturnsEmptyGuid()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var command = new AddOrderCommand
            {
                UserId = userId
            };

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CartItems = new List<CartItem>()
            };

            _mockUnitOfWork
                .SetupGet(x => x.CartRepository)
                .Returns(_mockCartRepository.Object);

            _mockCartRepository
                .Setup(x => x.GetByUserIdWithItemsAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(Guid.Empty),

                () => _mockCartRepository.Verify(
                    x => x.GetByUserIdWithItemsAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockTransactionManager.Verify(
                    x => x.BeginTransactionAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }



        // ============================================================
        // TEST 4: Valid order
        // ============================================================

        [Test]
        public async Task Handle_ValidCommand_CreatesOrderAndSaves()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var command = new AddOrderCommand
            {
                UserId = userId,
                FirstName = "Tushar",
                LastName = "Basak",
                Email = "tushar@example.com",
                Phone = "01700000000",
                Address = "Dhaka",
                TownCity = "Dhaka",
                CountryState = "Dhaka",
                PostcodeZip = "1207",
                OrderNotes = "Test order",
                PaymentType = PaymentType.CashOnDelivery
            };

            var product = new Product
            {
                ProductName = "Shirt"
            };

            var variant = new ProductVariant
            {
                Id = variantId,
                Sku = "SHIRT-001",
                Color = "Black",
                Size = "M",
                Product = product
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                ProductVariantId = variantId,
                ProductVariant = variant,
                Quantity = 2,
                UnitPrice = 1000m
            };

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CartItems = new List<CartItem>
                {
                    cartItem
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.CartRepository)
                .Returns(_mockCartRepository.Object);

            _mockUnitOfWork
                .SetupGet(x => x.InventoryRepository)
                .Returns(_mockInventoryRepository.Object);

            _mockUnitOfWork
                .SetupGet(x => x.OrderRepository)
                .Returns(_mockOrderRepository.Object);

            _mockCartRepository
                .Setup(x => x.GetByUserIdWithItemsAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _mockInventoryRepository
                .Setup(x => x.DecreaseStockAsync(
                    variantId,
                    2,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockTransactionManager
                .Setup(x => x.BeginTransactionAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockOrderRepository
                .Setup(x => x.AddAsync(
                    It.IsAny<Order>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockCartRepository
                .Setup(x => x.ClearAsync(
                    cart.Id,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockTransactionManager
                .Setup(x => x.CommitTransactionAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBe(Guid.Empty),

                () => _mockCartRepository.Verify(
                    x => x.GetByUserIdWithItemsAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.DecreaseStockAsync(
                        variantId,
                        2,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockOrderRepository.Verify(
                    x => x.AddAsync(
                        It.Is<Order>(order =>
                            order.UserId == userId &&
                            order.Subtotal == 2000m &&
                            order.DiscountAmount == 0m &&
                            order.Total == 2000m &&
                            order.OrderItems.Count == 1 &&
                            order.Payment != null),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockCartRepository.Verify(
                    x => x.ClearAsync(
                        cart.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockTransactionManager.Verify(
                    x => x.BeginTransactionAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockTransactionManager.Verify(
                    x => x.CommitTransactionAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }



        // ============================================================
        // TEST 5: Stock unavailable
        // ============================================================

        [Test]
        public async Task Handle_StockUnavailable_RollsBackAndReturnsEmptyGuid()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var command = new AddOrderCommand
            {
                UserId = userId,
                PaymentType = PaymentType.CashOnDelivery
            };

            var product = new Product
            {
                ProductName = "Shirt"
            };

            var variant = new ProductVariant
            {
                Id = variantId,
                Sku = "SHIRT-001",
                Color = "Black",
                Size = "M",
                Product = product
            };

            var cartItem = new CartItem
            {
                ProductVariantId = variantId,
                ProductVariant = variant,
                Quantity = 2,
                UnitPrice = 1000m
            };

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CartItems = new List<CartItem>
                {
                    cartItem
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.CartRepository)
                .Returns(_mockCartRepository.Object);

            _mockUnitOfWork
                .SetupGet(x => x.InventoryRepository)
                .Returns(_mockInventoryRepository.Object);

            _mockCartRepository
                .Setup(x => x.GetByUserIdWithItemsAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _mockInventoryRepository
                .Setup(x => x.DecreaseStockAsync(
                    variantId,
                    2,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockTransactionManager
                .Setup(x => x.BeginTransactionAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockTransactionManager
                .Setup(x => x.RollbackTransactionAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(Guid.Empty),

                () => _mockTransactionManager.Verify(
                    x => x.BeginTransactionAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.DecreaseStockAsync(
                        variantId,
                        2,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockTransactionManager.Verify(
                    x => x.RollbackTransactionAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockOrderRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<Order>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }
    }
}



