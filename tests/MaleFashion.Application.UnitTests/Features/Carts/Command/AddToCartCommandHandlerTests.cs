using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Carts.Command;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Carts.Command
{
   

   public class AddToCartCommandHandlerTests
    {
        private AutoMock _mock;

        private AddToCartCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<IProductRepository> _mockProductRepository;

        private Mock<ICartRepository> _mockCartRepository;

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

            _mockProductRepository =
                _mock.Mock<IProductRepository>();

            _mockCartRepository =
                _mock.Mock<ICartRepository>();

            _handler =
                _mock.Create<AddToCartCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockCartRepository?.Reset();

            _mockProductRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_AddsCartItemAndReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var productId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var command = new AddToCartCommand
            {
                UserId = userId,

                ProductId = productId,

                ProductVariantId = variantId,

                Quantity = 2
            };

            var product = new Product
            {
                Id = productId,

                ProductPrize = 1500m
            };

            var variant = new ProductVariant
            {
                Id = variantId,

                ProductId = productId,

                IsActive = true,

                Product = product,

                Inventory = new Inventory
                {
                    IsActive = true,

                    Quantity = 10
                }
            };

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockUnitOfWork
                .SetupGet(x => x.CartRepository)
                .Returns(_mockCartRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            _mockCartRepository
                .Setup(x => x.GetCartByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _mockCartRepository
                .Setup(x => x.GetCartItemAsync(
                    cart.Id,
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartItem?)null);

            _mockCartRepository
                .Setup(x => x.AddCartItemAsync(
                    It.IsAny<CartItem>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeTrue(),
                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        variantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockCartRepository.Verify(
                    x => x.GetCartItemAsync(
                        cart.Id,
                        variantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockCartRepository.Verify(
                    x => x.AddCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_InvalidCommand_ReturnsFalse()
        {
            // Arrange
            var command = new AddToCartCommand
            {
                UserId = Guid.Empty,

                ProductId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = 1
            };

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),
                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }

        [Test]
        public async Task Handle_VariantNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new AddToCartCommand
            {
                UserId = Guid.NewGuid(),

                ProductId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = 1
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    command.ProductVariantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductVariant?)null);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),
                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        command.ProductVariantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }

        [Test]
        public async Task Handle_VariantBelongsToDifferentProduct_ReturnsFalse()
        {
            // Arrange
            var command = new AddToCartCommand
            {
                UserId = Guid.NewGuid(),

                ProductId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = 1
            };

            var variant = new ProductVariant
            {
                Id = command.ProductVariantId,

                ProductId = Guid.NewGuid(),

                IsActive = true,

                Inventory = new Inventory
                {
                    IsActive = true,

                    Quantity = 10
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    command.ProductVariantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),
                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        command.ProductVariantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }

        [Test]
        public async Task Handle_VariantInactive_ReturnsFalse()
        {
            // Arrange
            var command = new AddToCartCommand
            {
                UserId = Guid.NewGuid(),

                ProductId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = 1
            };

            var variant = new ProductVariant
            {
                Id = command.ProductVariantId,

                ProductId = command.ProductId,

                IsActive = false,

                Inventory = new Inventory
                {
                    IsActive = true,

                    Quantity = 10
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    command.ProductVariantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),
                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        command.ProductVariantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }

        [Test]
        public async Task Handle_InsufficientInventory_ReturnsFalse()
        {
            // Arrange
            var command = new AddToCartCommand
            {
                UserId = Guid.NewGuid(),

                ProductId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = 10
            };

            var variant = new ProductVariant
            {
                Id = command.ProductVariantId,

                ProductId = command.ProductId,

                IsActive = true,

                Inventory = new Inventory
                {
                    IsActive = true,

                    Quantity = 5
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    command.ProductVariantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),
                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        command.ProductVariantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),
                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
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