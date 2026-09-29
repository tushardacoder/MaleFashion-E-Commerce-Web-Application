using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Command
{

    public class AddToCartCommandHandler
        : ICommandHandler<AddToCartCommand, bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            AddToCartCommand command,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // VALIDATION
            // ==========================================

            if (command.UserId == Guid.Empty)
                return false;

            if (command.ProductId == Guid.Empty)
                return false;

            if (command.ProductVariantId == Guid.Empty)
                return false;

            if (command.Quantity <= 0)
                return false;

           
            // ==========================================
            // GET PRODUCT VARIANT + INVENTORY
            // ==========================================

            var variant =
                await _unitOfWork
                    .ProductRepository
                    .GetVariantInventoryByIdAsync(
                        command.ProductVariantId,
                        cancellationToken);

            if (variant == null)
                return false;


            // ==========================================
            // CHECK PRODUCT
            // ==========================================

            if (variant.ProductId != command.ProductId)
                return false;


            // ==========================================
            // CHECK VARIANT
            // ==========================================

            if (!variant.IsActive)
                return false;


            // ==========================================
            // CHECK INVENTORY
            // ==========================================

            if (variant.Inventory == null)
                return false;

            if (!variant.Inventory.IsActive)
                return false;

            if (variant.Inventory.Quantity < command.Quantity)
                return false;


            // ==========================================
            // GET USER CART
            // ==========================================

            var cart =
                await _unitOfWork
                    .CartRepository
                    .GetCartByUserIdAsync(
                        command.UserId,
                        cancellationToken);


            // ==========================================
            // CREATE CART
            // ==========================================

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = IdentityGenerator.NewSequentialGuid(),

                    UserId = command.UserId
                };

                await _unitOfWork
                    .CartRepository
                    .AddCartAsync(
                        cart,
                        cancellationToken);

                await _unitOfWork.SaveAsync(
                    cancellationToken);
            }


            // ==========================================
            // GET EXISTING CART ITEM
            // ==========================================

            var existingItem =
                await _unitOfWork
                    .CartRepository
                    .GetCartItemAsync(
                        cart.Id,
                        command.ProductVariantId,
                        cancellationToken);


            // ==========================================
            // UPDATE EXISTING ITEM
            // ==========================================

            if (existingItem != null)
            {
                var newQuantity =
                    existingItem.Quantity +
                    command.Quantity;

                if (newQuantity >
                    variant.Inventory.Quantity)
                {
                    return false;
                }

                existingItem.Quantity =
                    newQuantity;

                existingItem.UnitPrice =
                    variant.Product.ProductPrize;
            }


            // ==========================================
            // CREATE NEW CART ITEM
            // ==========================================

            else
            {
                var cartItem = new CartItem
                {
                    Id = IdentityGenerator.NewSequentialGuid(),

                    CartId = cart.Id,

                    ProductVariantId =
                        command.ProductVariantId,

                    Quantity =
                        command.Quantity,

                    UnitPrice =
                        variant.Product.ProductPrize
                };

                await _unitOfWork
                    .CartRepository
                    .AddCartItemAsync(
                        cartItem,
                        cancellationToken);
            }


            // ==========================================
            // SAVE
            // ==========================================

            await _unitOfWork.SaveAsync(
                cancellationToken);

            return true;
        }


    }
}

