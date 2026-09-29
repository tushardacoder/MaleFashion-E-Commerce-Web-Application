using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Categories.Command;
using MaleFashion.Domain.Entities;
using MapsterMapper;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Categories.Commands
{

    public class CategoryAddCommandHandlerTests
    {
        private AutoMock _mock = null!;
        private CategoryAddCommandHandler _handler = null!;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork = null!;
        private Mock<ICategoryRepository> _mockCategoryRepository = null!;
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

            _mockCategoryRepository =
                _mock.Mock<ICategoryRepository>();

            _mockMapper =
                _mock.Mock<IMapper>();

            _handler =
                _mock.Create<CategoryAddCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockMapper.Reset();
            _mockCategoryRepository.Reset();
            _mockUnitOfWork.Reset();
        }

        // ============================================================
        // TEST 1: Unique category name
        // ============================================================

        [Test]
        public async Task Handle_UniqueCategoryName_AddsCategory()
        {
            // Arrange
            var command = new CategoryAddCommand
            {
                CategoryName = "Shirts",
                IsActive = true
            };

            var category = new Category
            {
                CategoryName = command.CategoryName,
                IsActive = command.IsActive
            };

            _mockUnitOfWork
                .SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _mockCategoryRepository
                .Setup(x => x.IsDuplicateCategoryName(
                    command.CategoryName,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockMapper
                .Setup(x => x.Map<Category>(command))
                .Returns(category);

            _mockCategoryRepository
                .Setup(x => x.AddAsync(
                    category,
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
                () => result.CategoryName.ShouldBe(command.CategoryName),
                () => result.IsActive.ShouldBe(command.IsActive),
                () => result.Id.ShouldNotBe(Guid.Empty),

                () => _mockCategoryRepository.Verify(
                    x => x.IsDuplicateCategoryName(
                        command.CategoryName,
                        null,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockMapper.Verify(
                    x => x.Map<Category>(command),
                    Times.Once),

                () => _mockCategoryRepository.Verify(
                    x => x.AddAsync(
                        category,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        // ============================================================
        // TEST 2: Duplicate category name
        // ============================================================

        [Test]
        public async Task Handle_DuplicateCategoryName_ReturnsNull()
        {
            // Arrange
            var command = new CategoryAddCommand
            {
                CategoryName = "Shirts",
                IsActive = true
            };

            _mockUnitOfWork
                .SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _mockCategoryRepository
                .Setup(x => x.IsDuplicateCategoryName(
                    command.CategoryName,
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

                // Duplicate name should stop the process.
                () => _mockCategoryRepository.Verify(
                    x => x.IsDuplicateCategoryName(
                        command.CategoryName,
                        null,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                // Mapper should never be called.
                () => _mockMapper.Verify(
                    x => x.Map<Category>(
                        It.IsAny<CategoryAddCommand>()),
                    Times.Never),

                // Category should never be added.
                () => _mockCategoryRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<Category>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                // Database should never be saved.
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }
    }
}
