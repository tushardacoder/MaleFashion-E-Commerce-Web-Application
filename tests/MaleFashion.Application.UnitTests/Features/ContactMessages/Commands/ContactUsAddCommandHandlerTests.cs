using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.ContactMessages.Command;
using MaleFashion.Domain.Entities;
using MapsterMapper;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.ContactMessages.Commands
{
    public class ContactUsAddCommandHandlerTests
    {
        private AutoMock _mock;
        private ContactUsAddCommandHandler _handler ;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork ;
        private Mock<IContactUsRepository> _mockContactUsRepository ;
        private Mock<IMapper> _mockMapper ;

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

            _mockContactUsRepository =
                _mock.Mock<IContactUsRepository>();

            _mockMapper =
                _mock.Mock<IMapper>();

            _handler =
                _mock.Create<ContactUsAddCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockMapper.Reset();
            _mockContactUsRepository.Reset();
            _mockUnitOfWork.Reset();
        }

        // ============================================================
        // TEST: Valid Contact Us
        // ============================================================
 

        [Test]
        public async Task Handle_ValidContactUs_AddsContactUs()
        {
            // Arrange
            var command = new ContactUsAddCommand
            {
                Name = "John Doe",
                Email = "john@example.com",
                Message = "I want more information about your product."
            };

            var contactUs = new ContactUs
            {
                Name = command.Name,
                Email = command.Email,
                Message = command.Message
            };

            _mockUnitOfWork
                .SetupGet(x => x.ContactUsRepository)
                .Returns(_mockContactUsRepository.Object);

            _mockMapper
                .Setup(x => x.Map<ContactUs>(command))
                .Returns(contactUs);

            _mockContactUsRepository
                .Setup(x => x.AddAsync(
                    contactUs,
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

             
                () => result.Id.ShouldNotBe(Guid.Empty),

               
                () => result.CreatedAt
                    .ShouldNotBe(default(DateTime)),

                () => result.Name
                    .ShouldBe(command.Name),

                () => result.Email
                    .ShouldBe(command.Email),

                () => result.Message
                    .ShouldBe(command.Message),

                
                () => _mockMapper.Verify(
                    x => x.Map<ContactUs>(command),
                    Times.Once),

                
                () => _mockContactUsRepository.Verify(
                    x => x.AddAsync(
                        contactUs,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

              
                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }
    }
}
