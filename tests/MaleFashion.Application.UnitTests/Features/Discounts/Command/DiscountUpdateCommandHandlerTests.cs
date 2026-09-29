using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Discounts.Command;
using MaleFashion.Domain.Entities;
using MapsterMapper;
using Moq;
using NUnit.Framework.Internal;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Discounts.Command
{
    public class DiscountUpdateCommandHandlerTests
    {
        private AutoMock _mock = null!;
        private DiscountUpdateCommandHandler _handler = null!;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork = null!;
        private Mock<IDiscountRepository> _mockDiscountRepository = null!;
        private Mock<IMapper> _mockMapper = null!;

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

            _mockMapper =
                _mock.Mock<IMapper>();

            _handler =
                _mock.Create<DiscountUpdateCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockDiscountRepository.Reset();
            _mockMapper.Reset();
            _mockUnitOfWork.Reset();
        }


        // =========================================================
        // VALID UPDATE
        // =========================================================

        [Test]
        public async Task Handle_ValidCommand_UpdatesDiscountAndSaves()
        {
            var discountId = Guid.NewGuid();

            var startAt = DateTime.UtcNow;
            var endAt = startAt.AddDays(7);

            var command = new DiscountUpdateCommand
            {
                Id = discountId,
                DiscountName = "Old Sale2",
                Code = "OlD2",
                DiscountPercentage = 20,
                StartAt = startAt,
                EndAt = endAt,
                IsActive = true
            };

            var discount = new Discount
            {
                Id = discountId,
                DiscountName = "Old Sale",
                Code = "OLD10",
                DiscountPercentage = 10,
                StartAt = startAt.AddDays(-5),
                EndAt = startAt.AddDays(2),
                IsActive = false
            };

            var mappedDiscount = new Discount
            {
                Id = discountId,
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
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockDiscountRepository
                .Setup(x => x.GetById(command.Id))
                .Returns(discount);

            _mockMapper
                .Setup(x => x.Map(
                    command,
                    discount))
                .Returns(mappedDiscount);

            _mockDiscountRepository
                .Setup(x => x.EditAsync(
                    mappedDiscount,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(command.Id),
                () => result.DiscountName.ShouldBe(command.DiscountName),
                () => result.Code.ShouldBe(command.Code),
                () => result.DiscountPercentage.ShouldBe(command.DiscountPercentage),
                () => result.StartAt.ShouldBe(command.StartAt),
                () => result.EndAt.ShouldBe(command.EndAt),
                () => result.IsActive.ShouldBe(command.IsActive),

                () => _mockDiscountRepository.Verify(
                    x => x.IsDuplicateDiscountCode(
                        command.Code,
                        command.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockDiscountRepository.Verify(
                    x => x.GetById(command.Id),
                    Times.Once),

                () => _mockMapper.Verify(
                    x => x.Map(
                        command,
                        discount),
                    Times.Once),

                () => _mockDiscountRepository.Verify(
                    x => x.EditAsync(
                        mappedDiscount,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }


        // =========================================================
        // DUPLICATE CODE
        // =========================================================

        [Test]
        public async Task Handle_DuplicateCode_ReturnsNull()
        {
            var discountId = Guid.NewGuid();

            var command = new DiscountUpdateCommand
            {
                Id = discountId,
                DiscountName = "Summer Sale",
                Code = "SUMMER20",
                DiscountPercentage = 20,
                StartAt = DateTime.UtcNow,
                EndAt = DateTime.UtcNow.AddDays(7),
                IsActive = true
            };

            _mockUnitOfWork
                .SetupGet(x => x.DiscountRepository)
                .Returns(_mockDiscountRepository.Object);

            _mockDiscountRepository
                .Setup(x => x.IsDuplicateDiscountCode(
                    command.Code,
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                () => _mockDiscountRepository.Verify(
                    x => x.IsDuplicateDiscountCode(
                        command.Code,
                        command.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockDiscountRepository.Verify(
                    x => x.GetById(
                        It.IsAny<Guid>()),
                    Times.Never),

                () => _mockMapper.Verify(
                    x => x.Map(
                        It.IsAny<DiscountUpdateCommand>(),
                        It.IsAny<Discount>()),
                    Times.Never),

                () => _mockDiscountRepository.Verify(
                    x => x.EditAsync(
                        It.IsAny<Discount>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        // =========================================================
        // INVALID DATE RANGE
        // =========================================================

        [Test]
        public async Task Handle_EndDateBeforeStartDate_ReturnsNull()
        {
            var discountId = Guid.NewGuid();

            var startAt = DateTime.UtcNow;
            var endAt = startAt.AddDays(-1);

            var command = new DiscountUpdateCommand
            {
                Id = discountId,
                DiscountName = "Summer Sale",
                Code = "SUMMER20",
                DiscountPercentage = 20,
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
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                () => _mockDiscountRepository.Verify(
                    x => x.IsDuplicateDiscountCode(
                        command.Code,
                        command.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockDiscountRepository.Verify(
                    x => x.GetById(
                        It.IsAny<Guid>()),
                    Times.Never),

                () => _mockMapper.Verify(
                    x => x.Map(
                        It.IsAny<DiscountUpdateCommand>(),
                        It.IsAny<Discount>()),
                    Times.Never),

                () => _mockDiscountRepository.Verify(
                    x => x.EditAsync(
                        It.IsAny<Discount>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }


        // =========================================================
        // DISCOUNT NOT FOUND
        // =========================================================

        [Test]
        public async Task Handle_DiscountNotFound_ReturnsNull()
        {
            var discountId = Guid.NewGuid();

            var command = new DiscountUpdateCommand
            {
                Id = discountId,
                DiscountName = "Summer Sale",
                Code = "SUMMER20",
                DiscountPercentage = 20,
                StartAt = DateTime.UtcNow,
                EndAt = DateTime.UtcNow.AddDays(7),
                IsActive = true
            };

            _mockUnitOfWork
                .SetupGet(x => x.DiscountRepository)
                .Returns(_mockDiscountRepository.Object);

            _mockDiscountRepository
                .Setup(x => x.IsDuplicateDiscountCode(
                    command.Code,
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockDiscountRepository
                .Setup(x => x.GetById(command.Id))
                .Returns((Discount?)null);

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                () => _mockDiscountRepository.Verify(
                    x => x.IsDuplicateDiscountCode(
                        command.Code,
                        command.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockDiscountRepository.Verify(
                    x => x.GetById(command.Id),
                    Times.Once),

                () => _mockMapper.Verify(
                    x => x.Map(
                        It.IsAny<DiscountUpdateCommand>(),
                        It.IsAny<Discount>()),
                    Times.Never),

                () => _mockDiscountRepository.Verify(
                    x => x.EditAsync(
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




