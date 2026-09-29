using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Features.Inventories.Command;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Command
{
    public class InventoryUpdateCommandHandler
       : ICommandHandler<
           InventoryUpdateCommand,
           Inventory?>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public InventoryUpdateCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Inventory?> Handle(
            InventoryUpdateCommand command,
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
                return null;
            }


            var duplicate =
                await _unitOfWork
                    .InventoryRepository
                    .GetCountAsync(
                        x =>
                            x.ProductVariantId ==
                                command.ProductVariantId
                            &&
                            x.Id != command.Id,
                        cancellationToken) > 0;


            if (duplicate)
            {
                return null;
            }


            inventory.ProductVariantId =
                command.ProductVariantId;

            inventory.Quantity =
                command.Quantity;

            inventory.IsActive =
                command.IsActive;

            inventory.UpdatedAt =
                DateTime.UtcNow;


            await _unitOfWork
                .InventoryRepository
                .EditAsync(
                    inventory,
                    cancellationToken);


            await _unitOfWork
                .SaveAsync(cancellationToken);


            return inventory;
        }
    }
}

