using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Wishlists.Command;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Wishlists.Commands
{
    public class AddToWishlistCommandHandlerTests
    {
        private AutoMock _mock;

        private AddToWishlistCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<IWishlistRepository> _mockWishlistRepository;

        private Mock<IProductRepository> _mockProductRepository;


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


            _mockProductRepository =
                _mock.Mock<IProductRepository>();


            _handler =
                _mock.Create<AddToWishlistCommandHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockProductRepository?.Reset();

            _mockWishlistRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        // =========================================================
        // VALID USER + VALID PRODUCT VARIANT
        // =========================================================

        [Test]
        public async Task Handle_ValidCommand_AddsWishlistAndSaves()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var userId =
                Guid.NewGuid();

            var productVariantId =
                Guid.NewGuid();


            var command = new AddToWishlistCommand
            {
                UserId = userId,

                ProductVariantId =
                    productVariantId
            };


            var variant =
                new ProductVariant
                {
                    Id = productVariantId
                };


            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object)
                .Verifiable();


            _mockUnitOfWork
                .SetupGet(x => x.WishlistRepository)
                .Returns(_mockWishlistRepository.Object)
                .Verifiable();


            _mockProductRepository
                .Setup(x => x.GetVariantByIdAsync(
                    command.ProductVariantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant)
                .Verifiable();


            _mockWishlistRepository
                .Setup(x => x.AddAsync(
                    It.Is<WishList>(wishlist =>
                        wishlist.UserId == command.UserId
                        && wishlist.Items != null
                        && wishlist.Items.Count == 1
                        && wishlist.Items.First().ProductVariantId
                            == command.ProductVariantId),
                    It.IsAny<CancellationToken>()))
                .Verifiable();


            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Verifiable();


            // =========================================================
            // ACT
            // =========================================================

            var result =
                await _handler.Handle(
                    command,
                    default);


            // =========================================================
            // ASSERT
            // =========================================================

            this.ShouldSatisfyAllConditions(

                () => result.ShouldBeTrue(),

                () => _mockProductRepository.VerifyAll(),

                () => _mockWishlistRepository.VerifyAll(),

                () => _mockUnitOfWork.VerifyAll()
            );
        }


        // =========================================================
        // EMPTY USER ID
        // =========================================================

        [Test]
        public async Task Handle_EmptyUserId_ThrowsArgumentException()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var command = new AddToWishlistCommand
            {
                UserId = Guid.Empty,

                ProductVariantId =
                    Guid.NewGuid()
            };


            // =========================================================
            // ACT
            // =========================================================

            var exception =
                await Should.ThrowAsync<ArgumentException>(
                    async () =>
                        await _handler.Handle(
                            command,
                            default));


            // =========================================================
            // ASSERT
            // =========================================================

            this.ShouldSatisfyAllConditions(

                () => exception.Message
                    .ShouldBe("User ID is required."),

                () => _mockProductRepository.Verify(
                    x => x.GetVariantByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockWishlistRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<WishList>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        // =========================================================
        // EMPTY PRODUCT VARIANT ID
        // =========================================================

        [Test]
        public async Task Handle_EmptyProductVariantId_ThrowsArgumentException()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var command = new AddToWishlistCommand
            {
                UserId =
                    Guid.NewGuid(),

                ProductVariantId =
                    Guid.Empty
            };


            // =========================================================
            // ACT
            // =========================================================

            var exception =
                await Should.ThrowAsync<ArgumentException>(
                    async () =>
                        await _handler.Handle(
                            command,
                            default));


            // =========================================================
            // ASSERT
            // =========================================================

            this.ShouldSatisfyAllConditions(

                () => exception.Message
                    .ShouldBe(
                        "Product variant ID is required."),

                () => _mockProductRepository.Verify(
                    x => x.GetVariantByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockWishlistRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<WishList>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        // =========================================================
        // PRODUCT VARIANT NOT FOUND
        // =========================================================

        [Test]
        public async Task Handle_ProductVariantNotFound_ThrowsException()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var command = new AddToWishlistCommand
            {
                UserId =
                    Guid.NewGuid(),

                ProductVariantId =
                    Guid.NewGuid()
            };


            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object)
                .Verifiable();


            _mockProductRepository
                .Setup(x => x.GetVariantByIdAsync(
                    command.ProductVariantId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductVariant)null)
                .Verifiable();


            // =========================================================
            // ACT
            // =========================================================

            var exception =
                await Should.ThrowAsync<Exception>(
                    async () =>
                        await _handler.Handle(
                            command,
                            default));


            // =========================================================
            // ASSERT
            // =========================================================

            this.ShouldSatisfyAllConditions(

                () => exception.Message
                    .ShouldBe(
                        "Product variant was not found."),

                () => _mockProductRepository.VerifyAll(),

                () => _mockWishlistRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<WishList>(),
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
