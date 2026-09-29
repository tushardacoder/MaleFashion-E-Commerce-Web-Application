using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Products.Command;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Products.Commands
{
  public class ProductDeleteCommandHandlerTests
    {
        private AutoMock _mock;

        private ProductDeleteCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

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

            _mockProductRepository =
                _mock.Mock<IProductRepository>();

            _handler =
                _mock.Create<ProductDeleteCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockProductRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_RemovesProductAndSaves()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var command = new ProductDeleteCommand
            {
                Id = productId
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.RemoveAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(

                () => _mockProductRepository.Verify(
                    x => x.RemoveAsync(
                        productId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_ValidCommand_PassesCorrectProductId()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var command = new ProductDeleteCommand
            {
                Id = productId
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.RemoveAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(

                () => _mockProductRepository.Verify(
                    x => x.RemoveAsync(
                        productId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockProductRepository.Verify(
                    x => x.RemoveAsync(
                        It.Is<Guid>(id => id == productId),
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

       

      
        
    }
}
