using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Inventories.Command;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Command
{
    public class InventoryAddCommandHandler
       : ICommandHandler<InventoryAddCommand, Inventory?>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public InventoryAddCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Inventory?> Handle(
            InventoryAddCommand command,
            CancellationToken cancellationToken)
        {
            var exists =
                await _unitOfWork
                    .InventoryRepository
                    .GetCountAsync(
                        x =>
                            x.ProductVariantId ==
                            command.ProductVariantId,
                        cancellationToken) > 0;


            if (exists)
            {
                return null; 
            }


            var inventory = new Inventory
            {
                Id =
                    IdentityGenerator.NewSequentialGuid(),

                ProductVariantId =
                    command.ProductVariantId,

                Quantity =
                    command.Quantity,

                UpdatedAt =
                    DateTime.UtcNow,

                IsActive =
                    command.IsActive
            };


            await _unitOfWork
                .InventoryRepository
                .AddAsync(
                    inventory,
                    cancellationToken);


            await _unitOfWork
                .SaveAsync(cancellationToken);


            return inventory;
        }
    }
}


