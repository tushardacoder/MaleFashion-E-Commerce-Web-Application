using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Command
{

    public class DiscountAddCommandHandler
        : ICommandHandler<DiscountAddCommand, Discount>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiscountAddCommandHandler(
            IApplicationUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Discount> Handle(
            DiscountAddCommand command,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // DUPLICATE CODE CHECK
            // ==========================================

            var isDuplicate =
                await _unitOfWork
                    .DiscountRepository
                    .IsDuplicateDiscountCode(
                        command.Code,
                        null,
                        cancellationToken);

            if (isDuplicate)
            {
                return null;
            }


            // ==========================================
            // DATE VALIDATION
            // ==========================================

            if (command.EndAt < command.StartAt)
            {
                return null;
            }


            // ==========================================
            // MAP COMMAND → ENTITY
            // ==========================================

            var discount =
                _mapper.Map<Discount>(command);


            // ==========================================
            // GENERATE ID
            // ==========================================

            discount.Id =
                IdentityGenerator.NewSequentialGuid();


            // ==========================================
            // ADD
            // ==========================================

            await _unitOfWork
                .DiscountRepository
                .AddAsync(
                    discount,
                    cancellationToken);


            // ==========================================
            // SAVE
            // ==========================================

            await _unitOfWork
                .SaveAsync(cancellationToken);


            return discount;
        }
    }
}
