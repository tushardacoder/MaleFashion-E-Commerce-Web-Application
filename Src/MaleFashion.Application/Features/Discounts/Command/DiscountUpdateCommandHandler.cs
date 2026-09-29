using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Command
{
    public class DiscountUpdateCommandHandler
      : ICommandHandler<DiscountUpdateCommand, Discount>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiscountUpdateCommandHandler(
            IApplicationUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Discount> Handle(
            DiscountUpdateCommand command,
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
                        command.Id,
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
            // GET DISCOUNT
            // ==========================================

            var discount =
                _unitOfWork
                    .DiscountRepository
                    .GetById(command.Id);


            if (discount == null)
            {
                return null;
            }


            // ==========================================
            // MAP COMMAND → ENTITY
            // ==========================================

            discount =
                _mapper.Map(
                    command,
                    discount);


            // ==========================================
            // UPDATE
            // ==========================================

            await _unitOfWork
                .DiscountRepository
                .EditAsync(
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
