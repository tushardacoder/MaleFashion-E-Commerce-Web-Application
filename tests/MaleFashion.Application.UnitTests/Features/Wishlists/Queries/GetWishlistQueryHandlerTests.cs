using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Wishlists.Query;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Wishlists.Queries
{
    public class GetWishlistQueryHandlerTests
    {
        private AutoMock _mock;

        private GetWishlistQueryHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<IWishlistRepository> _mockWishlistRepository;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mock = AutoMock.GetLoose();
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


            _mockWishlistRepository =
                _mock.Mock<IWishlistRepository>();


            _handler =
                _mock.Create<GetWishlistQueryHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockWishlistRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        
        // EMPTY USER ID
        
        [Test]
        public async Task Handle_EmptyUserId_ReturnsEmptyWishlist()
        {
          
            // ARRANGE
      
            var query =
                new GetWishlistQuery(Guid.Empty);

         
            // ACT
      

            var result =
                await _handler.Handle(
                    query,
                    default);


          
            // ASSERT

            this.ShouldSatisfyAllConditions(

                () => result.ShouldNotBeNull(),

                () => result.Items.ShouldNotBeNull(),

                () => result.Items.ShouldBeEmpty(),

                () => _mockWishlistRepository.Verify(
                    x => x.GetByUserIdWithItemsAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        // =========================================================
        // NO WISHLISTS
        // =========================================================

        [Test]
        public async Task Handle_NoWishlists_ReturnsEmptyWishlist()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var userId =
                Guid.NewGuid();

            var query =
                new GetWishlistQuery(userId);


            _mockUnitOfWork
                .SetupGet(x => x.WishlistRepository)
                .Returns(_mockWishlistRepository.Object)
                .Verifiable();


            _mockWishlistRepository
                .Setup(x => x.GetByUserIdWithItemsAsync(
                    query.UserId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<WishList>())
                .Verifiable();


            // =========================================================
            // ACT
            // =========================================================

            var result =
                await _handler.Handle(
                    query,
                    default);


            // =========================================================
            // ASSERT
            // =========================================================

            this.ShouldSatisfyAllConditions(

                () => result.ShouldNotBeNull(),

                () => result.Items.ShouldNotBeNull(),

                () => result.Items.ShouldBeEmpty(),

                () => _mockWishlistRepository.VerifyAll(),

                () => _mockUnitOfWork.VerifyAll()
            );
        }


        // =========================================================
        // NULL WISHLISTS
        // =========================================================

        [Test]
        public async Task Handle_NullWishlists_ReturnsEmptyWishlist()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var userId =
                Guid.NewGuid();

            var query =
                new GetWishlistQuery(userId);


            _mockUnitOfWork
                .SetupGet(x => x.WishlistRepository)
                .Returns(_mockWishlistRepository.Object)
                .Verifiable();


            _mockWishlistRepository
                .Setup(x => x.GetByUserIdWithItemsAsync(
                    query.UserId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<WishList>)null)
                .Verifiable();


            // =========================================================
            // ACT
            // =========================================================

            var result =
                await _handler.Handle(
                    query,
                    default);


            // =========================================================
            // ASSERT
            // =========================================================

            this.ShouldSatisfyAllConditions(

                () => result.ShouldNotBeNull(),

                () => result.Items.ShouldNotBeNull(),

                () => result.Items.ShouldBeEmpty(),

                () => _mockWishlistRepository.VerifyAll(),

                () => _mockUnitOfWork.VerifyAll()
            );
        }


        // =========================================================
        // VALID WISHLIST ITEMS
        // =========================================================

        [Test]
        public async Task Handle_ValidWishlists_ReturnsMappedWishlistItems()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var userId =
                Guid.NewGuid();

            var wishlistId =
                Guid.NewGuid();

            var wishlistItemId =
                Guid.NewGuid();

            var productId =
                Guid.NewGuid();

            var variantId =
                Guid.NewGuid();

            var addedAt =
                DateTime.UtcNow;


            var image =
                new ProductImage
                {
                    Id =
                        Guid.NewGuid(),

                    ImageName =
                        "shirt.jpg",

                    DisplayOrder =
                        1
                };


            var product =
                new Product
                {
                    Id =
                        productId,

                    ProductName =
                        "Formal Shirt",

                    Branding =
                        "MaleFashion",

                    ProductPrize =
                        2500m
                };


            var variant =
                new ProductVariant
                {
                    Id =
                        variantId,

                    Product =
                        product,

                    Color =
                        "Black",

                    Size =
                        "L",

                    Sku =
                        "SHIRT-BLK-L",

                    Images =
                        new List<ProductImage>
                        {
                            image
                        },

                    Inventory =
                        new Inventory
                        {
                            Quantity =
                                10
                        }
                };


            var wishlistItem =
                new WishlistItem
                {
                    Id =
                        wishlistItemId,

                    WishlistId =
                        wishlistId,

                    ProductVariantId =
                        variantId,

                    ProductVariant =
                        variant,

                    AddedAt =
                        addedAt
                };


            var wishlist =
                new WishList
                {
                    Id =
                        wishlistId,

                    UserId =
                        userId,

                    Items =
                        new List<WishlistItem>
                        {
                            wishlistItem
                        }
                };


            var query =
                new GetWishlistQuery(userId);


            _mockUnitOfWork
                .SetupGet(x => x.WishlistRepository)
                .Returns(_mockWishlistRepository.Object)
                .Verifiable();


            _mockWishlistRepository
                .Setup(x => x.GetByUserIdWithItemsAsync(
                    query.UserId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<WishList>
                    {
                        wishlist
                    })
                .Verifiable();


            // =========================================================
            // ACT
            // =========================================================

            var result =
                await _handler.Handle(
                    query,
                    default);


            // =========================================================
            // ASSERT
            // =========================================================

            var item =
                result.Items.Single();


            this.ShouldSatisfyAllConditions(

                () => result.ShouldNotBeNull(),

                () => result.Items.ShouldNotBeNull(),

                () => result.Items.Count
                    .ShouldBe(1),

                () => item.Id
                    .ShouldBe(wishlistItemId),

                () => item.ProductId
                    .ShouldBe(productId),

                () => item.ProductVariantId
                    .ShouldBe(variantId),

                () => item.ProductName
                    .ShouldBe("Formal Shirt"),

                () => item.Branding
                    .ShouldBe("MaleFashion"),

                () => item.Color
                    .ShouldBe("Black"),

                () => item.Size
                    .ShouldBe("L"),

                () => item.Sku
                    .ShouldBe("SHIRT-BLK-L"),

                () => item.Price
                    .ShouldBe(2500m),

                () => item.Image
                    .ShouldBe("shirt.jpg"),

                () => item.AddedAt
                    .ShouldBe(addedAt),

                () => item.IsInStock
                    .ShouldBeTrue(),

                () => _mockWishlistRepository.VerifyAll(),

                () => _mockUnitOfWork.VerifyAll()
            );
        }

 }
    }
