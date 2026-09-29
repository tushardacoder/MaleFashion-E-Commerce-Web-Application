using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.ContactMessages.Command
{
    public class ContactUsAddCommandHandler: ICommandHandler<ContactUsAddCommand, ContactUs>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactUsAddCommandHandler(
            IApplicationUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ContactUs> Handle(
            ContactUsAddCommand command,
            CancellationToken cancellationToken)
        {
            var contactUs = _mapper.Map<ContactUs>(command);

            contactUs.Id = IdentityGenerator.NewSequentialGuid();
            contactUs.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.ContactUsRepository
                .AddAsync(contactUs, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return contactUs;
        }
    }
}
