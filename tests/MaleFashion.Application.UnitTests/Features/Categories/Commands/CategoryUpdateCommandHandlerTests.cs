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

    public class CategoryUpdateCommandHandlerTests
    {
        private AutoMock _mock = null!;
        private CategoryUpdateCommandHandler _handler = null!;

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
                _mock.Create<CategoryUpdateCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockMapper.Reset();
            _mockCategoryRepository.Reset();
            _mockUnitOfWork.Reset();
        }

        // ============================================================
        // TEST 1: Unique category name + category exists
        // ============================================================

        [Test]
        public async Task Handle_UniqueCategoryName_UpdatesCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var command = new CategoryUpdateCommand
            {
                Id = categoryId,
                CategoryName = "Updated Shirts",
                IsActive = true
            };

            var existingCategory = new Category
            {
                Id = categoryId,
                CategoryName = "Old Shirts",
                IsActive = false
            };

            var updatedCategory = new Category
            {
                Id = categoryId,
                CategoryName = command.CategoryName,
                IsActive = command.IsActive
            };

            _mockUnitOfWork
                .SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _mockCategoryRepository
                .Setup(x => x.IsDuplicateCategoryName(
                    command.CategoryName,
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockCategoryRepository
                .Setup(x => x.GetById(command.Id))
                .Returns(existingCategory);

            _mockMapper
                .Setup(x => x.Map(
                    command,
                    existingCategory))
                .Returns(updatedCategory);

            _mockCategoryRepository
                .Setup(x => x.EditAsync(
                    updatedCategory,
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

                () => result.Id.ShouldBe(categoryId),

                () => result.CategoryName
                    .ShouldBe(command.CategoryName),

                () => result.IsActive
                    .ShouldBe(command.IsActive),

                () => _mockCategoryRepository.Verify(
                    x => x.IsDuplicateCategoryName(
                        command.CategoryName,
                        command.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockCategoryRepository.Verify(
                    x => x.GetById(command.Id),
                    Times.Once),

                () => _mockMapper.Verify(
                    x => x.Map(
                        command,
                        existingCategory),
                    Times.Once),

                () => _mockCategoryRepository.Verify(
                    x => x.EditAsync(
                        updatedCategory,
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
            var categoryId = Guid.NewGuid();

            var command = new CategoryUpdateCommand
            {
                Id = categoryId,
                CategoryName = "Existing Category",
                IsActive = true
            };

            _mockUnitOfWork
                .SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _mockCategoryRepository
                .Setup(x => x.IsDuplicateCategoryName(
                    command.CategoryName,
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                // Duplicate check should happen once.
                () => _mockCategoryRepository.Verify(
                    x => x.IsDuplicateCategoryName(
                        command.CategoryName,
                        command.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                // Handler should stop here.
                () => _mockCategoryRepository.Verify(
                    x => x.GetById(It.IsAny<Guid>()),
                    Times.Never),

                () => _mockMapper.Verify(
                    x => x.Map(
                        It.IsAny<CategoryUpdateCommand>(),
                        It.IsAny<Category>()),
                    Times.Never),

                () => _mockCategoryRepository.Verify(
                    x => x.EditAsync(
                        It.IsAny<Category>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }

        // ============================================================
        // TEST 3: Category does not exist
        // ============================================================

        [Test]
        public async Task Handle_CategoryNotFound_ReturnsNull()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var command = new CategoryUpdateCommand
            {
                Id = categoryId,
                CategoryName = "New Shirts",
                IsActive = true
            };

            _mockUnitOfWork
                .SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            _mockCategoryRepository
                .Setup(x => x.IsDuplicateCategoryName(
                    command.CategoryName,
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockCategoryRepository
                .Setup(x => x.GetById(command.Id))
                .Returns((Category)null!);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBeNull(),

                // Duplicate check happens.
                () => _mockCategoryRepository.Verify(
                    x => x.IsDuplicateCategoryName(
                        command.CategoryName,
                        command.Id,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                // Category lookup happens.
                () => _mockCategoryRepository.Verify(
                    x => x.GetById(command.Id),
                    Times.Once),

                // Since category doesn't exist, update should stop.
                () => _mockMapper.Verify(
                    x => x.Map(
                        It.IsAny<CategoryUpdateCommand>(),
                        It.IsAny<Category>()),
                    Times.Never),

                () => _mockCategoryRepository.Verify(
                    x => x.EditAsync(
                        It.IsAny<Category>(),
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

