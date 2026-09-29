using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Wishlists.Command
{
    public class AddToWishlistCommandHandler
       : ICommandHandler<
           AddToWishlistCommand,
           bool>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public AddToWishlistCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> Handle(
     AddToWishlistCommand command,
     CancellationToken cancellationToken)
        {


            // ==========================================
            // VALIDATION
            // ==========================================

            if (command.UserId == Guid.Empty)
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            if (command.ProductVariantId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Product variant ID is required.");
            }


            // ==========================================
            // CHECK PRODUCT VARIANT
            // ==========================================

            var variant =
                await _unitOfWork
                    .ProductRepository
                    .GetVariantByIdAsync(
                        command.ProductVariantId,
                        cancellationToken);

            if (variant == null)
            {
                throw new Exception(
                    "Product variant was not found.");
            }


            // ==========================================
            // CREATE NEW WISHLIST
            // ==========================================

            var wishlist = new WishList
            {
                Id = IdentityGenerator.NewSequentialGuid(),

                UserId = command.UserId,

                Items = new List<WishlistItem>()
            };


            // ==========================================
            // CREATE WISHLIST ITEM
            // ==========================================

            var wishlistItem = new WishlistItem
            {
                Id = IdentityGenerator.NewSequentialGuid(),

                WishlistId = wishlist.Id,

                ProductVariantId =
                    command.ProductVariantId,

                AddedAt = DateTime.UtcNow
            };


            // ==========================================
            // ADD ITEM TO WISHLIST
            // ==========================================

            wishlist.Items.Add(wishlistItem);


            // ==========================================
            // ADD WISHLIST
            // ==========================================

            await _unitOfWork
                .WishlistRepository
                .AddAsync(
                    wishlist,
                    cancellationToken);


            // ==========================================
            // SAVE
            // ==========================================

            await _unitOfWork.SaveAsync(
                cancellationToken);


            return true;
        }
    }
}



