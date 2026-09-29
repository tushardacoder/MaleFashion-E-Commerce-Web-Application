using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Command
{
    public class DiscountDeleteCommandHandler
       : ICommandHandler<DiscountDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public DiscountDeleteCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            DiscountDeleteCommand command,
            CancellationToken cancellationToken)
        {
            await _unitOfWork
                .DiscountRepository
                .RemoveAsync(
                    command.Id,
                    cancellationToken);


            await _unitOfWork
                .SaveAsync(cancellationToken);
        }
    }
}
