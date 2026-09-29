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

    public class UpdateCartQuantityCommandHandlerTests
    {
        private AutoMock _mock;

        private UpdateCartQuantityCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<ICartRepository> _mockCartRepository;

        private Mock<IProductRepository> _mockProductRepository;

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

            _mockProductRepository =
                _mock.Mock<IProductRepository>();

            _mockUnitOfWork
                .SetupGet(x => x.CartRepository)
                .Returns(_mockCartRepository.Object);

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _handler =
                _mock.Create<UpdateCartQuantityCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockCartRepository?.Reset();

            _mockProductRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        [Test]
        public async Task Handle_ValidCommand_UpdatesCartItemAndReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cartId = Guid.NewGuid();

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId = variantId,

                Quantity = 3
            };

            var product = new Product
            {
                Id = Guid.NewGuid(),

                ProductPrize = 1500m
            };

            var cart = new Cart
            {
                Id = cartId,

                UserId = userId
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),

                CartId = cartId,

                ProductVariantId = variantId,

                Quantity = 1,

                UnitPrice = 1000m
            };

            var variant = new ProductVariant
            {
                Id = variantId,

                ProductId = product.Id,

                IsActive = true,

                Product = product,

                Inventory = new Inventory
                {
                    IsActive = true,

                    Quantity = 10
                }
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _mockCartRepository
                .Setup(x => x.GetCartItemAsync(
                    cartId,
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartItem);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            _mockCartRepository
                .Setup(x => x.UpdateCartItemAsync(
                    cartItem,
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

                () => cartItem.Quantity
                    .ShouldBe(3),

                () => cartItem.UnitPrice
                    .ShouldBe(1500m),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockCartRepository.Verify(
                    x => x.GetCartItemAsync(
                        cartId,
                        variantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        variantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        cartItem,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }


        [Test]
        public async Task Handle_EmptyUserId_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateCartQuantityCommand
            {
                UserId = Guid.Empty,

                ProductVariantId = Guid.NewGuid(),

                Quantity = 2
            };

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_EmptyProductVariantId_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateCartQuantityCommand
            {
                UserId = Guid.NewGuid(),

                ProductVariantId = Guid.Empty,

                Quantity = 2
            };

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_ZeroQuantity_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateCartQuantityCommand
            {
                UserId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = 0
            };

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_NegativeQuantity_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateCartQuantityCommand
            {
                UserId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = -1
            };

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_CartNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateCartQuantityCommand
            {
                UserId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid(),

                Quantity = 2
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        command.UserId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockCartRepository.Verify(
                    x => x.GetCartItemAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_CartItemNotFound_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId
            };

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId = variantId,

                Quantity = 2
            };

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

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.GetCartItemAsync(
                        cart.Id,
                        variantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockProductRepository.Verify(
                    x => x.GetVariantInventoryByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
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
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),

                CartId = cart.Id,

                ProductVariantId = variantId,

                Quantity = 1,

                UnitPrice = 1000m
            };

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId = variantId,

                Quantity = 2
            };

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
                .ReturnsAsync(cartItem);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    variantId,
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
                        variantId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_InventoryNotFound_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),

                CartId = cart.Id,

                ProductVariantId = variantId,

                Quantity = 1
            };

            var variant = new ProductVariant
            {
                Id = variantId,

                IsActive = true,

                Inventory = null
            };

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId = variantId,

                Quantity = 2
            };

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
                .ReturnsAsync(cartItem);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_InactiveInventory_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),

                CartId = cart.Id,

                ProductVariantId = variantId,

                Quantity = 1
            };

            var variant = new ProductVariant
            {
                Id = variantId,

                IsActive = true,

                Inventory = new Inventory
                {
                    IsActive = false,

                    Quantity = 10
                }
            };

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId = variantId,

                Quantity = 2
            };

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
                .ReturnsAsync(cartItem);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
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
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),

                CartId = cart.Id,

                ProductVariantId = variantId,

                Quantity = 1
            };

            var product = new Product
            {
                Id = Guid.NewGuid(),

                ProductPrize = 1500m
            };

            var variant = new ProductVariant
            {
                Id = variantId,

                ProductId = product.Id,

                IsActive = true,

                Product = product,

                Inventory = new Inventory
                {
                    IsActive = true,

                    Quantity = 5
                }
            };

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId = variantId,

                Quantity = 10
            };

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
                .ReturnsAsync(cartItem);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeFalse(),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        It.IsAny<CartItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_QuantityEqualsInventoryQuantity_UpdatesAndReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),

                CartId = cart.Id,

                ProductVariantId = variantId,

                Quantity = 1,

                UnitPrice = 1000m
            };

            var product = new Product
            {
                Id = Guid.NewGuid(),

                ProductPrize = 2000m
            };

            var variant = new ProductVariant
            {
                Id = variantId,

                ProductId = product.Id,

                IsActive = true,

                Product = product,

                Inventory = new Inventory
                {
                    IsActive = true,

                    Quantity = 5
                }
            };

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId = variantId,

                Quantity = 5
            };

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
                .ReturnsAsync(cartItem);

            _mockProductRepository
                .Setup(x => x.GetVariantInventoryByIdAsync(
                    variantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            _mockCartRepository
                .Setup(x => x.UpdateCartItemAsync(
                    cartItem,
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

                () => cartItem.Quantity
                    .ShouldBe(5),

                () => cartItem.UnitPrice
                    .ShouldBe(2000m),

                () => _mockCartRepository.Verify(
                    x => x.UpdateCartItemAsync(
                        cartItem,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }
    }
}