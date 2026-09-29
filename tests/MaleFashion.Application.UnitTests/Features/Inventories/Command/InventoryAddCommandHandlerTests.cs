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
using System.Linq.Expressions;

namespace MaleFashion.Application.UnitTests.Features.Inventories.Command
{
    
  public class InventoryAddCommandHandlerTests
    {
        private AutoMock _mock;

        private InventoryAddCommandHandler _handler;

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
                _mock.Create<InventoryAddCommandHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockInventoryRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }


        [Test]
        public async Task Handle_ValidCommand_AddsInventoryAndReturnsInventory()
        {
            // Arrange
            var productVariantId =
                Guid.NewGuid();

            var command = new InventoryAddCommand
            {
                ProductVariantId =
                    productVariantId,

                Quantity =
                    10,

                IsActive =
                    true
            };

            _mockUnitOfWork
                .SetupGet(x => x.InventoryRepository)
                .Returns(_mockInventoryRepository.Object);

            _mockInventoryRepository
                .Setup(x => x.GetCountAsync(
                    It.IsAny<Expression<Func<Inventory, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _mockInventoryRepository
                .Setup(x => x.AddAsync(
                    It.IsAny<Inventory>(),
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

                () => result!.ProductVariantId
                    .ShouldBe(productVariantId),

                () => result!.Quantity
                    .ShouldBe(10),

                () => result!.IsActive
                    .ShouldBeTrue(),

                () => result!.UpdatedAt
                    .ShouldNotBe(default(DateTime)),

                () => _mockInventoryRepository.Verify(
                    x => x.GetCountAsync(
                        It.IsAny<Expression<Func<Inventory, bool>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<Inventory>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }


        [Test]
        public async Task Handle_InventoryAlreadyExists_ReturnsNull()
        {
            // Arrange
            var productVariantId =
                Guid.NewGuid();

            var command = new InventoryAddCommand
            {
                ProductVariantId =
                    productVariantId,

                Quantity =
                    10,

                IsActive =
                    true
            };

            _mockUnitOfWork
                .SetupGet(x => x.InventoryRepository)
                .Returns(_mockInventoryRepository.Object);

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
                    x => x.GetCountAsync(
                        It.IsAny<Expression<Func<Inventory, bool>>>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockInventoryRepository.Verify(
                    x => x.AddAsync(
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