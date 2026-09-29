using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Inventories.Command;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Inventories.Command
{
    
    public class InventoryUpdateCommandHandlerTests
    {
        private AutoMock _mock;

        private InventoryUpdateCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<IInventoryRepository> _mockInventoryRepository;


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

            _mockInventoryRepository =
                _mock.Mock<IInventoryRepository>();

            _handler =
                _mock.Create<InventoryUpdateCommandHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockInventoryRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        [Test]
        public async Task Handle_ValidCommand_UpdatesInventoryAndReturnsInventory()
        {
            // Arrange
            var inventoryId =
                Guid.NewGuid();

            var productVariantId =
                Guid.NewGuid();

            var command = new InventoryUpdateCommand
            {
                Id =
                    inventoryId,

                ProductVariantId =
                    productVariantId,

                Quantity =
                    20,

                IsActive =
                    true
            };

            var inventory = new Inventory
            {
                Id =
                    inventoryId,

                ProductVariantId =
                    Guid.NewGuid(),

                Quantity =
                    10,

                IsActive =
                    false,

                UpdatedAt =
                    DateTime.UtcNow.AddDays(-1)
            };

            _mockUnitOfWork
                .SetupGet(x => x.InventoryRepository)
                .Returns(_mockInventoryRepository.Object);

            _mockInventoryRepository
                .Setup(x => x.GetByIdAsync(
                    inventoryId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(inventory);

            _mockInventoryRepository
                .Setup(x => x.GetCountAsync(
                    It.IsAny<Expression<Func<Inventory, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _mockInventoryRepository
                .Setup(x => x.EditAsync(
                    inventory,
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
                () => result.ShouldNotBeNull(),

                () => result!.Id
                    .ShouldBe(inventoryId),

                () => result!.ProductVariantId
                    .ShouldBe(productVariantId),

                () => result!.Quantity
                    .ShouldBe(20),

                () => result!.IsActive
                    .ShouldBeTrue(),

                () => result!.UpdatedAt
                    .ShouldNotBe(default(DateTime)),

                () => _mockInventoryRepository.Verify(
                    x => x.GetByIdAsync(
                        inventoryId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.GetCountAsync(
                        It.IsAny<Expression<Func<Inventory, bool>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.EditAsync(
                        inventory,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }


        [Test]
        public async Task Handle_InventoryNotFound_ReturnsNull()
        {
            // Arrange
            var inventoryId =
                Guid.NewGuid();

            var command = new InventoryUpdateCommand
            {
                Id =
                    inventoryId,

                ProductVariantId =
                    Guid.NewGuid(),

                Quantity =
                    20,

                IsActive =
                    true
            };

            _mockUnitOfWork
                .SetupGet(x => x.InventoryRepository)
                .Returns(_mockInventoryRepository.Object);

            _mockInventoryRepository
                .Setup(x => x.GetByIdAsync(
                    inventoryId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Inventory?)null);


            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);


            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                () => _mockInventoryRepository.Verify(
                    x => x.GetByIdAsync(
                        inventoryId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.GetCountAsync(
                        It.IsAny<Expression<Func<Inventory, bool>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockInventoryRepository.Verify(
                    x => x.EditAsync(
                        It.IsAny<Inventory>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        [Test]
        public async Task Handle_DuplicateProductVariant_ReturnsNull()
        {
            // Arrange
            var inventoryId =
                Guid.NewGuid();

            var productVariantId =
                Guid.NewGuid();

            var command = new InventoryUpdateCommand
            {
                Id =
                    inventoryId,

                ProductVariantId =
                    productVariantId,

                Quantity =
                    20,

                IsActive =
                    true
            };

            var inventory = new Inventory
            {
                Id =
                    inventoryId,

                ProductVariantId =
                    Guid.NewGuid(),

                Quantity =
                    10,

                IsActive =
                    true
            };

            _mockUnitOfWork
                .SetupGet(x => x.InventoryRepository)
                .Returns(_mockInventoryRepository.Object);

            _mockInventoryRepository
                .Setup(x => x.GetByIdAsync(
                    inventoryId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(inventory);

            _mockInventoryRepository
                .Setup(x => x.GetCountAsync(
                    It.IsAny<Expression<Func<Inventory, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);


            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);


            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                () => _mockInventoryRepository.Verify(
                    x => x.GetByIdAsync(
                        inventoryId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.GetCountAsync(
                        It.IsAny<Expression<Func<Inventory, bool>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.EditAsync(
                        It.IsAny<Inventory>(),
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