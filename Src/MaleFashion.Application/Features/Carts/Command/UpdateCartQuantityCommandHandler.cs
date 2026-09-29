using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Command
{
    public class UpdateCartQuantityCommandHandler
       : ICommandHandler<UpdateCartQuantityCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public UpdateCartQuantityCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateCartQuantityCommand command,
            CancellationToken cancellationToken)
        {
            if (command.UserId == Guid.Empty)
                return false;

            if (command.ProductVariantId == Guid.Empty)
                return false;

            if (command.Quantity <= 0)
                return false;


            var cart =
                await _unitOfWork
                    .CartRepository
                    .GetCartByUserIdAsync(
                        command.UserId,
                        cancellationToken);

            if (cart == null)
                return false;


            var cartItem =
                await _unitOfWork
                    .CartRepository
                    .GetCartItemAsync(
                        cart.Id,
                        command.ProductVariantId,
                        cancellationToken);

            if (cartItem == null)
                return false;


            var variant =
                await _unitOfWork
                    .ProductRepository
                    .GetVariantInventoryByIdAsync(
                        command.ProductVariantId,
                        cancellationToken);

            if (variant == null)
                return false;

            if (variant.Inventory == null)
                return false;

            if (!variant.Inventory.IsActive)
                return false;


            if (command.Quantity >
                variant.Inventory.Quantity)
            {
                return false;
            }


            cartItem.Quantity =
                command.Quantity;

            cartItem.UnitPrice =
                variant.Product.ProductPrize;


            await _unitOfWork
                .CartRepository
                .UpdateCartItemAsync(
                    cartItem,
                    cancellationToken);


            await _unitOfWork.SaveAsync(
                cancellationToken);


            return true;
        }
    }

}






