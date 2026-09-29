using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Categories.Command;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Categories.Commands
{
    public class CategoryDeleteCommandHandlerTests
    {


        private AutoMock _mock;
        private CategoryDeleteCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<ICategoryRepository> _mockCategoryRepository;



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


            _mockCategoryRepository =
                _mock.Mock<ICategoryRepository>();


            _handler =
                _mock.Create<CategoryDeleteCommandHandler>();
        }



        [TearDown]
        public void TearDown()
        {

            _mockUnitOfWork?.Reset();


            _mockCategoryRepository?.Reset();
        }



        [Test]
        public async Task Handle_ValidCategoryId_RemovesCategoryBeforeSaving()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var command = new CategoryDeleteCommand
            {
                Id = categoryId
            };

            var cancellationToken = new CancellationToken();

            _mockUnitOfWork
                .SetupGet(x => x.CategoryRepository)
                .Returns(_mockCategoryRepository.Object);

            var sequence = new MockSequence();

            _mockCategoryRepository
                .InSequence(sequence)
                .Setup(x => x.RemoveAsync(
                    categoryId,
                    cancellationToken));

            _mockUnitOfWork
                .InSequence(sequence)
                .Setup(x => x.SaveAsync(
                    cancellationToken));

            // Act
            await _handler.Handle(
                command,
                cancellationToken);

            // Assert

            this.ShouldSatisfyAllConditions(
                    () => _mockCategoryRepository.Verify(
                    x => x.RemoveAsync(
                    categoryId,
                    cancellationToken),
                   Times.Once),

           () => _mockUnitOfWork.Verify(
                   x => x.SaveAsync(
                   cancellationToken),
                   Times.Once)
              );
        }
    }
}
