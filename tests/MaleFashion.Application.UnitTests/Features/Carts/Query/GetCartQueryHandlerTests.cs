using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Carts.Query;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Carts.Query
{
   
    public class GetCartQueryHandlerTests
    {
        private AutoMock _mock;

        private GetCartQueryHandler _handler;

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
                _mock.Create<GetCartQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockCartRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        [Test]
        public async Task Handle_EmptyUserId_ReturnsEmptyCart()
        {
            // Arrange
            var query = new GetCartQuery
            {
                UserId = Guid.Empty
            };

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),

                () => result.Items
                    .ShouldBeEmpty(),

                () => result.Subtotal
                    .ShouldBe(0),

                () => result.Discount
                    .ShouldBe(0),

                () => result.Total
                    .ShouldBe(0),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_CartNotFound_ReturnsEmptyCart()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var query = new GetCartQuery
            {
                UserId = userId
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),

                () => result.Items
                    .ShouldBeEmpty(),

                () => result.Subtotal
                    .ShouldBe(0),

                () => result.Discount
                    .ShouldBe(0),

                () => result.Total
                    .ShouldBe(0),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }


        [Test]
        public async Task Handle_ValidCart_ReturnsMappedCart()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var cartId = Guid.NewGuid();

            var variantId = Guid.NewGuid();

            var productId = Guid.NewGuid();

            var imageId = Guid.NewGuid();

            var query = new GetCartQuery
            {
                UserId = userId
            };

            var product = new Product
            {
                Id = productId,

                ProductName = "Classic T-Shirt"
            };

            var variant = new ProductVariant
            {
                Id = variantId,

                ProductId = productId,

                Product = product,

                Size = "L",

                Color = "Black",

                Sku = "TS-BLK-L",

                Inventory = new Inventory
                {
                    Quantity = 10,

                    IsActive = true
                },

                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        Id = imageId,

                        ImageName = "shirt.jpg"
                    }
                }
            };

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),

                CartId = cartId,

                ProductVariantId = variantId,

                ProductVariant = variant,

                UnitPrice = 1500m,

                Quantity = 2
            };

            var cart = new Cart
            {
                Id = cartId,

                UserId = userId,

                CartItems = new List<CartItem>
                {
                    cartItem
                }
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),

                () => result.Items.Count
                    .ShouldBe(1),

                () => result.Items[0].ProductVariantId
                    .ShouldBe(variantId),

                () => result.Items[0].ProductName
                    .ShouldBe("Classic T-Shirt"),

                () => result.Items[0].UnitPrice
                    .ShouldBe(1500m),

                () => result.Items[0].Quantity
                    .ShouldBe(2),

                () => result.Items[0].Size
                    .ShouldBe("L"),

                () => result.Items[0].Color
                    .ShouldBe("Black"),

                () => result.Items[0].Sku
                    .ShouldBe("TS-BLK-L"),

                () => result.Items[0].Stock
                    .ShouldBe(10),

                () => result.Items[0].ImageName
                    .ShouldBe("shirt.jpg"),

                () => result.Items[0].Total
                    .ShouldBe(3000m),

                () => result.Subtotal
                    .ShouldBe(3000m),

                () => result.Discount
                    .ShouldBe(0),

                () => result.Total
                    .ShouldBe(3000m),

                () => _mockCartRepository.Verify(
                    x => x.GetCartByUserIdAsync(
                        userId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }




        [Test]
        public async Task Handle_MultipleCartItems_CalculatesCorrectSubtotalAndTotal()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var product1 = new Product
            {
                Id = Guid.NewGuid(),

                ProductName = "T-Shirt",

                ProductPrize = 1000m
            };

            var product2 = new Product
            {
                Id = Guid.NewGuid(),

                ProductName = "Jeans",

                ProductPrize = 2000m
            };

            var variant1 = new ProductVariant
            {
                Id = Guid.NewGuid(),

                Product = product1,

                Inventory = new Inventory
                {
                    Quantity = 10,

                    IsActive = true
                }
            };

            var variant2 = new ProductVariant
            {
                Id = Guid.NewGuid(),

                Product = product2,

                Inventory = new Inventory
                {
                    Quantity = 5,

                    IsActive = true
                }
            };

            var cart = new Cart
            {
                Id = Guid.NewGuid(),

                UserId = userId,

                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        Id = Guid.NewGuid(),

                        ProductVariantId = variant1.Id,

                        ProductVariant = variant1,

                        UnitPrice = 1000m,

                        Quantity = 2
                    },

                    new CartItem
                    {
                        Id = Guid.NewGuid(),

                        ProductVariantId = variant2.Id,

                        ProductVariant = variant2,

                        UnitPrice = 2000m,

                        Quantity = 3
                    }
                }
            };

            var query = new GetCartQuery
            {
                UserId = userId
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.Items.Count
                    .ShouldBe(2),

                () => result.Items[0].Total
                    .ShouldBe(2000m),

                () => result.Items[1].Total
                    .ShouldBe(6000m),

                () => result.Subtotal
                    .ShouldBe(8000m),

                () => result.Discount
                    .ShouldBe(0),

                () => result.Total
                    .ShouldBe(8000m)
            );
        }


       

     }
}