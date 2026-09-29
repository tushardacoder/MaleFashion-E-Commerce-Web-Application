using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Discounts.Command;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Discounts.Command
{
    public class DiscountDeleteCommandHandlerTests
    {
        private AutoMock _mock = null!;
        private DiscountDeleteCommandHandler _handler = null!;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork = null!;
        private Mock<IDiscountRepository> _mockDiscountRepository = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork =
                _mock.Mock<IApplicationUnitOfWork>();

            _mockDiscountRepository =
                _mock.Mock<IDiscountRepository>();

            _handler =
                _mock.Create<DiscountDeleteCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockDiscountRepository.Reset();
            _mockUnitOfWork.Reset();
        }

        // ============================================================
        // TEST: Delete discount
        // ============================================================

        [Test]
        public async Task Handle_ValidCommand_DeletesDiscountAndSaves()
        {
            // Arrange
            var discountId = Guid.NewGuid();

            var command = new DiscountDeleteCommand
            {
                Id = discountId
            };

            var cancellationToken = CancellationToken.None;

            _mockUnitOfWork
                .SetupGet(x => x.DiscountRepository)
                .Returns(_mockDiscountRepository.Object);

            _mockDiscountRepository
                .Setup(x => x.RemoveAsync(
                    command.Id,
                    cancellationToken))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(
                command,
                cancellationToken);

            // Assert
            this.ShouldSatisfyAllConditions(

                // RemoveAsync should be called exactly once.
                () => _mockDiscountRepository.Verify(
                    x => x.RemoveAsync(
                        command.Id,
                        cancellationToken),
                    Times.Once),

                // SaveAsync should be called exactly once.
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        cancellationToken),
                    Times.Once)
            );
        }
    }
}
