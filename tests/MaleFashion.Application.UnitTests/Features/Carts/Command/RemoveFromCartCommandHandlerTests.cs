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
  
    public class RemoveFromCartCommandHandlerTests
    {
        private AutoMock _mock;

        private RemoveFromCartCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

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

            _mockCartRepository =
                _mock.Mock<ICartRepository>();

            _mockUnitOfWork
                .SetupGet(x => x.CartRepository)
                .Returns(_mockCartRepository.Object);

            _handler =
                _mock.Create<RemoveFromCartCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockCartRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        [Test]
        public async Task Handle_ValidCommand_RemovesCartItemAndReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var cartId = Guid.NewGuid();

            var command = new RemoveFromCartCommand
            {
                UserId = userId,

                ProductVariantId = variantId
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

                Quantity = 2,

                UnitPrice = 1500m
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

            _mockCartRepository
                .Setup(x => x.DeleteCartItemAsync(
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

                () => _mockCartRepository.Verify(
                    x => x.DeleteCartItemAsync(
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
            var command = new RemoveFromCartCommand
            {
                UserId = Guid.Empty,

                ProductVariantId = Guid.NewGuid()
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
                    x => x.GetCartItemAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.DeleteCartItemAsync(
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
            var command = new RemoveFromCartCommand
            {
                UserId = Guid.NewGuid(),

                ProductVariantId = Guid.Empty
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
                    x => x.GetCartItemAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockCartRepository.Verify(
                    x => x.DeleteCartItemAsync(
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
            var command = new RemoveFromCartCommand
            {
                UserId = Guid.NewGuid(),

                ProductVariantId = Guid.NewGuid()
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

                () => _mockCartRepository.Verify(
                    x => x.DeleteCartItemAsync(
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

            var command = new RemoveFromCartCommand
            {
                UserId = userId,

                ProductVariantId = variantId
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
                    x => x.DeleteCartItemAsync(
                        It.IsAny<CartItem>(),
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