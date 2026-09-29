using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Features.Inventories.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Command
{
    public class InventoryDeleteCommandHandler
        : ICommandHandler<InventoryDeleteCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public InventoryDeleteCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> Handle(
            InventoryDeleteCommand command,
            CancellationToken cancellationToken)
        {
            var inventory =
                await _unitOfWork
                    .InventoryRepository
                    .GetByIdAsync(
                        command.Id,
                        cancellationToken);


            if (inventory == null)
            {
                return false;
            }


            await _unitOfWork
                .InventoryRepository
                .RemoveAsync(
                    inventory,
                    cancellationToken);


            await _unitOfWork
                .SaveAsync(cancellationToken);


            return true;
        }
    }
}

