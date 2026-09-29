using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Discounts.Command;
using MaleFashion.Domain.Entities;
using MapsterMapper;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Discounts.Command
{
    public class DiscountAddCommandHandlerTests
    {
        private AutoMock _mock;

        private DiscountAddCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<IDiscountRepository> _mockDiscountRepository;

        private Mock<IMapper> _mockMapper;


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

            _mockDiscountRepository =
                _mock.Mock<IDiscountRepository>();

            _mockMapper =
                _mock.Mock<IMapper>();

            _handler =
                _mock.Create<DiscountAddCommandHandler>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockMapper?.Reset();

            _mockDiscountRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }



        // ============================================================
        // TEST 1: Valid command
        // ============================================================

        [Test]
        public async Task Handle_ValidCommand_AddsDiscountAndSaves()
        {
            // Arrange
            var command = new DiscountAddCommand
            {
                DiscountName = "Summer Discount",
                Code = "SUMMER20",
                DiscountPercentage = 20m,
                StartAt = DateTime.UtcNow,
                EndAt = DateTime.UtcNow.AddDays(10),
                IsActive = true
            };

            var discount = new Discount
            {
                DiscountName = command.DiscountName,
                Code = command.Code,
                DiscountPercentage = command.DiscountPercentage,
                StartAt = command.StartAt,
                EndAt = command.EndAt,
                IsActive = command.IsActive
            };

            _mockUnitOfWork
                .SetupGet(x => x.DiscountRepository)
                .Returns(_mockDiscountRepository.Object);

            _mockDiscountRepository
                .Setup(x => x.IsDuplicateDiscountCode(
                    command.Code,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockMapper
                .Setup(x => x.Map<Discount>(command))
                .Returns(discount);

            _mockDiscountRepository
                .Setup(x => x.AddAsync(
                    discount,
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

                () => result.DiscountName
                    .ShouldBe(command.DiscountName),

                () => result.Code
                    .ShouldBe(command.Code),

                () => result.DiscountPercentage
                    .ShouldBe(command.DiscountPercentage),

                () => result.StartAt
                    .ShouldBe(command.StartAt),

                () => result.EndAt
                    .ShouldBe(command.EndAt),

                () => result.IsActive
                    .ShouldBe(command.IsActive),

                () => result.Id
                    .ShouldNotBe(Guid.Empty),

                () => _mockDiscountRepository.Verify(
                    x => x.IsDuplicateDiscountCode(
                        command.Code,
                        null,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockMapper.Verify(
                    x => x.Map<Discount>(command),
                    Times.Once),

                () => _mockDiscountRepository.Verify(
                    x => x.AddAsync(
                        discount,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        // ============================================================
        // TEST 2: Duplicate discount code
        // ============================================================

        [Test]
        public async Task Handle_DuplicateDiscountCode_ReturnsNull()
        {
            // Arrange
            var command = new DiscountAddCommand
            {
                DiscountName = "Summer Discount",
                Code = "SUMMER20",
                DiscountPercentage = 20m,
                StartAt = DateTime.UtcNow,
                EndAt = DateTime.UtcNow.AddDays(10),
                IsActive = true
            };

            _mockUnitOfWork
                .SetupGet(x => x.DiscountRepository)
                .Returns(_mockDiscountRepository.Object);

            _mockDiscountRepository
                .Setup(x => x.IsDuplicateDiscountCode(
                    command.Code,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                () => _mockDiscountRepository.Verify(
                    x => x.IsDuplicateDiscountCode(
                        command.Code,
                        null,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockMapper.Verify(
                    x => x.Map<Discount>(
                        It.IsAny<DiscountAddCommand>()),
                    Times.Never),

                () => _mockDiscountRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<Discount>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }

        // ============================================================
        // TEST 3: Invalid date range
        // ============================================================

        [Test]
        public async Task Handle_EndAtBeforeStartAt_ReturnsNull()
        {
            // Arrange
            var startAt = DateTime.UtcNow;
            var endAt = startAt.AddDays(-1);

            var command = new DiscountAddCommand
            {
                DiscountName = "Invalid Discount",
                Code = "INVALID20",
                DiscountPercentage = 20m,
                StartAt = startAt,
                EndAt = endAt,
                IsActive = true
            };

            _mockUnitOfWork
                .SetupGet(x => x.DiscountRepository)
                .Returns(_mockDiscountRepository.Object);

            _mockDiscountRepository
                .Setup(x => x.IsDuplicateDiscountCode(
                    command.Code,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                () => _mockDiscountRepository.Verify(
                    x => x.IsDuplicateDiscountCode(
                        command.Code,
                        null,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockMapper.Verify(
                    x => x.Map<Discount>(
                        It.IsAny<DiscountAddCommand>()),
                    Times.Never),

                () => _mockDiscountRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<Discount>(),
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


