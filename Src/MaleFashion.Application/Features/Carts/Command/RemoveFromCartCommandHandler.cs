using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Command
{
    public class RemoveFromCartCommandHandler
       : ICommandHandler<RemoveFromCartCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public RemoveFromCartCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> Handle(
            RemoveFromCartCommand command,
            CancellationToken cancellationToken)
        {
            // =====================================================
            // VALIDATION
            // =====================================================

            if (command.UserId == Guid.Empty)
                return false;

            if (command.ProductVariantId == Guid.Empty)
                return false;


            // =====================================================
            // GET CART
            // =====================================================

            var cart =
                await _unitOfWork
                    .CartRepository
                    .GetCartByUserIdAsync(
                        command.UserId,
                        cancellationToken);


            if (cart == null)
                return false;


            // =====================================================
            // GET CART ITEM
            // =====================================================

            var cartItem =
                await _unitOfWork
                    .CartRepository
                    .GetCartItemAsync(
                        cart.Id,
                        command.ProductVariantId,
                        cancellationToken);


            if (cartItem == null)
                return false;


            // =====================================================
            // DELETE
            // =====================================================

            await _unitOfWork
                .CartRepository
                .DeleteCartItemAsync(
                    cartItem,
                    cancellationToken);


            // =====================================================
            // SAVE
            // =====================================================

            await _unitOfWork.SaveAsync(
                cancellationToken);


            return true;
        }
    }
}



