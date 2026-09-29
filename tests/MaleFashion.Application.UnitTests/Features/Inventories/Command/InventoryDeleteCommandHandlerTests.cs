using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Inventories.Command;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Inventories.Command
{
    
    public class InventoryDeleteCommandHandlerTests
    {
        private AutoMock _mock;

        private InventoryDeleteCommandHandler _handler;

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
                _mock.Create<InventoryDeleteCommandHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockInventoryRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        [Test]
        public async Task Handle_ValidCommand_RemovesInventoryAndReturnsTrue()
        {
            // Arrange
            var inventoryId =
                Guid.NewGuid();

            var command = new InventoryDeleteCommand
            {
                Id =
                    inventoryId
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
                .Setup(x => x.RemoveAsync(
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
                () => result.ShouldBeTrue(),

                () => _mockInventoryRepository.Verify(
                    x => x.GetByIdAsync(
                        inventoryId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.RemoveAsync(
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
        public async Task Handle_InventoryNotFound_ReturnsFalse()
        {
            // Arrange
            var inventoryId =
                Guid.NewGuid();

            var command = new InventoryDeleteCommand
            {
                Id =
                    inventoryId
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
                () => result.ShouldBeFalse(),

                () => _mockInventoryRepository.Verify(
                    x => x.GetByIdAsync(
                        inventoryId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.RemoveAsync(
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