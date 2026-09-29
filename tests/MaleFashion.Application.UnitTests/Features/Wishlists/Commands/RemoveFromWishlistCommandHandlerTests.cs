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
    public class RemoveFromWishlistCommandHandlerTests
    {
        private AutoMock _mock;

        private RemoveFromWishlistCommandHandler _handler;

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
                _mock.Create<RemoveFromWishlistCommandHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockWishlistRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        // =========================================================
        // VALID COMMAND
        // =========================================================

        [Test]
        public async Task Handle_ValidCommand_RemovesWishlistItemAndSaves()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var userId =
                Guid.NewGuid();

            var wishlistItemId =
                Guid.NewGuid();


            var command = new RemoveFromWishlistCommand
            {
                UserId = userId,

                WishlistItemId =
                    wishlistItemId
            };


            var wishlistItem =
                new WishlistItem
                {
                    Id = wishlistItemId
                };


            _mockUnitOfWork
                .SetupGet(x => x.WishlistRepository)
                .Returns(_mockWishlistRepository.Object)
                .Verifiable();


            _mockWishlistRepository
                .Setup(x => x.GetItemByIdForUserAsync(
                    command.WishlistItemId,
                    command.UserId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(wishlistItem)
                .Verifiable();


            _mockWishlistRepository
                .Setup(x => x.RemoveItemAsync(
                    wishlistItem,
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

            var command = new RemoveFromWishlistCommand
            {
                UserId =
                    Guid.Empty,

                WishlistItemId =
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
                    .ShouldBe(
                        "User ID is required."),

                () => _mockWishlistRepository.Verify(
                    x => x.GetItemByIdForUserAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockWishlistRepository.Verify(
                    x => x.RemoveItemAsync(
                        It.IsAny<WishlistItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        // =========================================================
        // EMPTY WISHLIST ITEM ID
        // =========================================================

        [Test]
        public async Task Handle_EmptyWishlistItemId_ThrowsArgumentException()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var command = new RemoveFromWishlistCommand
            {
                UserId =
                    Guid.NewGuid(),

                WishlistItemId =
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
                        "Wishlist item ID is required."),

                () => _mockWishlistRepository.Verify(
                    x => x.GetItemByIdForUserAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockWishlistRepository.Verify(
                    x => x.RemoveItemAsync(
                        It.IsAny<WishlistItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        // =========================================================
        // ITEM NOT FOUND / NOT OWNED BY USER
        // =========================================================

        [Test]
        public async Task Handle_WishlistItemNotFound_ReturnsFalse()
        {
            // =========================================================
            // ARRANGE
            // =========================================================

            var command = new RemoveFromWishlistCommand
            {
                UserId =
                    Guid.NewGuid(),

                WishlistItemId =
                    Guid.NewGuid()
            };


            _mockUnitOfWork
                .SetupGet(x => x.WishlistRepository)
                .Returns(_mockWishlistRepository.Object)
                .Verifiable();


            _mockWishlistRepository
                .Setup(x => x.GetItemByIdForUserAsync(
                    command.WishlistItemId,
                    command.UserId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((WishlistItem)null)
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

                () => result.ShouldBeFalse(),

                () => _mockWishlistRepository.VerifyAll(),

                () => _mockWishlistRepository.Verify(
                    x => x.RemoveItemAsync(
                        It.IsAny<WishlistItem>(),
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
